﻿// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.Tools.Formatters;
using Xunit;

namespace Microsoft.CodeAnalysis.Tools.Tests.Formatters
{
    public class CharsetFormatterTests : CSharpFormatterTests
    {
        private protected override ICodeFormatter Formatter => new CharsetFormatter();

        [Theory]
        [InlineData("latin1", "utf-8")]
        [InlineData("latin1", "utf-8-bom")]
        [InlineData("latin1", "utf-16be")]
        [InlineData("latin1", "utf-16le")]
        [InlineData("utf-8", "latin1")]
        [InlineData("utf-8", "utf-8-bom")]
        [InlineData("utf-8", "utf-16be")]
        [InlineData("utf-8", "utf-16le")]
        [InlineData("utf-8-bom", "latin1")]
        [InlineData("utf-8-bom", "utf-8")]
        [InlineData("utf-8-bom", "utf-16be")]
        [InlineData("utf-8-bom", "utf-16le")]
        [InlineData("utf-16be", "latin1")]
        [InlineData("utf-16be", "utf-8")]
        [InlineData("utf-16be", "utf-8-bom")]
        [InlineData("utf-16be", "utf-16le")]
        [InlineData("utf-16le", "latin1")]
        [InlineData("utf-16le", "utf-8")]
        [InlineData("utf-16le", "utf-8-bom")]
        [InlineData("utf-16le", "utf-16be")]
        public async Task TestCharsetWrong_CharsetFixed(string codeValue, string expectedValue)
        {
            var codeEncoding = CharsetFormatter.GetCharset(codeValue);
            var expectedEncoding = CharsetFormatter.GetCharset(expectedValue);

            var testCode = "class C { }";

            var editorConfig = new Dictionary<string, string>()
            {
                ["charset"] = expectedValue,
            };

            var formattedText = await TestAsync(testCode, testCode, editorConfig, codeEncoding);

            Assert.Equal(expectedEncoding, formattedText.Encoding);
        }

        [Fact]
        public async Task TestCharsetNotSpecified_NoChange()
        {
            // This encoding is not supported by .editorconfig
            var codeEncoding = Encoding.UTF32;

            var testCode = "class C { }";

            var editorConfig = new Dictionary<string, string>()
            {
            };

            var formattedText = await TestAsync(testCode, testCode, editorConfig, codeEncoding);

            Assert.Equal(codeEncoding, formattedText.Encoding);
        }
    }
}
