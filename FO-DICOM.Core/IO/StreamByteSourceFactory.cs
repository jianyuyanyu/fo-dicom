﻿// Copyright (c) 2012-2025 fo-dicom contributors.
// Licensed under the Microsoft Public License (MS-PL).
#nullable disable

using System.IO;

namespace FellowOakDicom.IO
{
    /// <summary>
    /// Factory for creating a stream byte source for reading.
    /// </summary>
    public static class StreamByteSourceFactory
    {
        /// <summary>
        /// Returns a newly created  instance of a stream byte source class.
        /// The actual class depends on the stream capabilities.
        /// </summary>
        /// <param name="stream">Stream to read from.</param>
        /// <param name="readOption">Defines the handling of large tags.</param>
        /// <param name="largeObjectSize">Custom limit of what are large values and what are not.
        /// If 0 is passed, then the default of 64k is used.</param>
        public static IByteSource Create(Stream stream, FileReadOption readOption = FileReadOption.Default,
            int largeObjectSize = 0)
        {
            if (stream.CanSeek)
            {
                return new StreamByteSource(stream, readOption, largeObjectSize);
            }

            // in case of unseekable stream, we need to add a buffer as wrapper. This allows the parser to do the necessary seek-operations.
            // a buffer-size of 4096 should be appropriate.
            // and because there is a buffer in between, the FileReadOption.ReadLargeOnDemand is not supported there.
            return new StreamByteSource(new ReadBufferedStream(stream, 4096), readOption == FileReadOption.SkipLargeTags ? FileReadOption.SkipLargeTags : FileReadOption.ReadAll, largeObjectSize);
        }
    }
}