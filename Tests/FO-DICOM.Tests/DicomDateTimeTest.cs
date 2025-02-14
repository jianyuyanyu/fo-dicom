﻿// Copyright (c) 2012-2025 fo-dicom contributors.
// Licensed under the Microsoft Public License (MS-PL).
#nullable disable

using System;
using Xunit;

namespace FellowOakDicom.Tests
{

    [Collection(TestCollections.General)]
    public class DicomDateTimeTest
    {
        [Fact]
        public void Get_String_ReturnStringOnDicomValidFormat()
        {
            var dataset =
                new DicomDataset(
                    new DicomUniqueIdentifier(DicomTag.SOPClassUID, DicomUID.SecondaryCaptureImageStorage),
                    new DicomUniqueIdentifier(DicomTag.SOPInstanceUID, "1.2.3"),
                    new DicomDateTime(DicomTag.AcquisitionDateTime, new DateTime(2016, 4, 20, 10, 20, 30)));

            var expected = $"20160420102030";
            var actual = dataset.GetString(DicomTag.AcquisitionDateTime);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Add_StringWithDicomValidTimeZone_ShouldNotThrow()
        {
            var dataset =
                new DicomDataset(
                    new DicomUniqueIdentifier(DicomTag.SOPClassUID, DicomUID.SecondaryCaptureImageStorage),
                    new DicomUniqueIdentifier(DicomTag.SOPInstanceUID, "1.2.3"));

            var zone = new DateTime(2016, 4, 20).ToString("yyyyMMddHHmmsszzz").Substring(14).Replace(":", string.Empty);

            var exception = Record.Exception(() => dataset.Add(DicomTag.AcquisitionDateTime, $"20160420102030{zone}"));
            Assert.Null(exception);

            var expected = new DateTime(2016, 4, 20, 10, 20, 30);
            var actual = dataset.GetSingleValue<DateTime>(DicomTag.AcquisitionDateTime);

            Assert.Equal(expected, actual);
        }


        public static TheoryData<DicomTag, DateTime, string> Datetimes = new TheoryData<DicomTag, DateTime, string> 
        { 
            // with Fraction
            { DicomTag.AcquisitionDateTime, new DateTime(2007, 6, 28, 15, 19, 45, 406, DateTimeKind.Unspecified), "20070628151945.406" },
            { DicomTag.AcquisitionDateTime, new DateTime(2007, 6, 28, 15, 19, 45, 400, DateTimeKind.Unspecified), "20070628151945.4" },
            { DicomTag.StudyTime, new DateTime(2007, 6, 28, 15, 19, 45, 406, DateTimeKind.Unspecified), "151945.406" },
            { DicomTag.StudyTime, new DateTime(2007, 6, 28, 15, 19, 45, 400, DateTimeKind.Unspecified), "151945.4" },
            // without Fraction
            { DicomTag.AcquisitionDateTime, new DateTime(2007, 6, 28, 15, 19, 45, DateTimeKind.Unspecified), "20070628151945" },
            { DicomTag.AcquisitionDateTime, new DateTime(2007, 6, 28, 15, 0, 0, DateTimeKind.Unspecified), "20070628150000" },
            { DicomTag.StudyTime, new DateTime(2007, 6, 28, 15, 19, 45, DateTimeKind.Unspecified), "151945" },
            { DicomTag.StudyTime, new DateTime(2007, 6, 28, 15, 0, 0, DateTimeKind.Unspecified), "150000" }
        };

        [Theory]
        [MemberData(nameof(Datetimes))]
        public void AddDatetimeWithOrWithoutFraction(DicomTag tag, DateTime date, string expectedString)
        {
            var dataset = new DicomDataset
            {
                { tag, date }
            };
            var actualDateString = dataset.GetString(tag);

            Assert.Equal(expectedString, actualDateString);
        }

    }
}
