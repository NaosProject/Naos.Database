// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DocumentGenerator.cs" company="Naos">
//    Copyright (c) Naos 2017. All Rights Reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.SqlServer
{
    using System;
    using System.Globalization;
    using System.IO;
    using System.Text.RegularExpressions;

    using OBeautifulCode.Validation.Recipes;

    /// <summary>
    /// Microsoft Word XML implementation of <see cref="IDocumentGenerator" />.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1063:ImplementIDisposableCorrectly", Justification = "Keeping this disposal.")]
    public class DocumentGenerator : IDocumentGenerator
    {
        private const int DefaultWidth = 120;

        private static readonly Regex WidthRegex = new Regex(@"\[\[W[0-9]{1,3}\]\]"); // ie. [[W100]]
        private static readonly Regex BoldRegex = new Regex(@"\[\[BOLD\]\]"); // ie. [[BOLD]]

        private readonly object syncClose = new object();
        private readonly string fileName;
        private readonly float marginIncrement = 0;

        private TextWriter fileWriter;
        private float margin = 0;
        private bool closed = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentGenerator"/> class.
        /// </summary>
        /// <param name="fileName">File name.</param>
        public DocumentGenerator(string fileName)
        {
            this.fileName = fileName;

            this.Init();
        }

        /// <inheritdoc cref="IDocumentGenerator" />
        public void AddEntry(string entry, int size, bool bold)
        {
            this.AddEntry(entry, size, bold, Alignment.Left);
        }

        /// <inheritdoc cref="IDocumentGenerator" />
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1308:NormalizeStringsToUppercase", Justification = "Keepign lower case.")]
        public void AddEntry(string entry, int size, bool bold, Alignment alignment)
        {
            new { entry }.Must().NotBeNullNorWhiteSpace();

            var marginInPoints = this.margin;
            if (bold)
            {
                this.fileWriter.WriteLine(
                    this.htmlParagraphBoldAddin.Replace(this.htmlCanaryMarginInPoints, marginInPoints.ToString(CultureInfo.InvariantCulture)).Replace(this.htmlCanaryValue, entry)
                        .Replace(this.htmlCanaryFontSizeInPoints, size.ToString(CultureInfo.CurrentCulture)).Replace(this.htmlCanaryAlign, alignment.ToString().ToLowerInvariant()));
            }
            else
            {
                this.fileWriter.WriteLine(
                    this.htmlParagraphAddin.Replace(this.htmlCanaryMarginInPoints, marginInPoints.ToString(CultureInfo.InvariantCulture)).Replace(this.htmlCanaryValue, entry)
                        .Replace(this.htmlCanaryFontSizeInPoints, size.ToString(CultureInfo.CurrentCulture)).Replace(this.htmlCanaryAlign, alignment.ToString().ToLowerInvariant()));
            }
        }

        /// <inheritdoc cref="IDocumentGenerator" />
        public void AddTable(string name, string[,] values, int[,] merges)
        {
            new { values }.Must().NotBeNull();

            var rowCount = values.GetLength(0);
            var columnCount = values.GetLength(1);

            var firstRow = true;
            if (!string.IsNullOrEmpty(name))
            {
                this.AddEntry(name, 11, true);
            }

            this.WriteTableHeader(this.margin + 5); // because tables are slightly offset to the left of paragraphs when margins are applied.
            for (int rowIdx = 0; rowIdx < rowCount; rowIdx++)
            {
                this.WriteRowHeader(firstRow, rowIdx);
                firstRow = false;
                int width = -1;
                int colSpan = 1;
                string colVal = string.Empty;
                for (int colIdx = 0; colIdx < columnCount; colIdx++)
                {
                    string val = values[rowIdx, colIdx];
                    val = val ?? string.Empty;
                    bool bold = false;

                    if (WidthRegex.IsMatch(val))
                    {
                        var matchWidth = WidthRegex.Match(val);
                        width += int.Parse(matchWidth.Value.Substring(3, matchWidth.Value.Length - 5), CultureInfo.CurrentCulture); // just the number portion
                        val = val.Replace(matchWidth.Value, string.Empty);
                    }

                    if (BoldRegex.IsMatch(val))
                    {
                        bold = true;
                        val = val.Replace("[[BOLD]]", string.Empty);
                    }

                    colVal += val;

                    if (InRowMergeRange(merges, rowIdx, colIdx))
                    {
                        colSpan++;
                    }
                    else
                    {
                        this.WriteTableElement(colVal, colSpan, bold, width);
                        colSpan = 1;
                        colVal = string.Empty;
                        width = -1;
                    }
                }

                this.WriteRowFooter();
            }

            this.WriteTableFooter();
        }

        private void WriteTableFooter()
        {
            this.fileWriter.WriteLine(this.htmlTableFooterAddin);
            this.fileWriter.WriteLine(this.htmlParagraphAddin.Replace(this.htmlCanaryFontSizeInPoints, "7").Replace(this.htmlCanaryMarginInPoints, "0").Replace(this.htmlCanaryValue, string.Empty));
        }

        private void WriteRowFooter()
        {
            this.fileWriter.WriteLine(this.htmlTableRowFooterAddin);
        }

        private void WriteTableHeader(float headerMargin)
        {
            this.fileWriter.WriteLine(this.htmlTableHeaderAddin.Replace(this.htmlCanaryMarginInPoints, headerMargin.ToString(CultureInfo.InvariantCulture)));
        }

        private void WriteRowHeader(bool firstRow, int rowIdx)
        {
            var entry = firstRow
                ? this.htmlTableFirstRowHeaderAddin
                : this.htmlTableRowHeaderAddin.Replace(this.htmlCanaryRowNumber, rowIdx.ToString(CultureInfo.CurrentCulture));

            this.fileWriter.WriteLine(entry);
        }

        private void WriteTableElement(string value, int columnSpan, bool bold, int width)
        {
            if (width == -1)
            {
                width = DefaultWidth;
            }

            if (bold)
            {
                this.fileWriter.WriteLine(
                    this.htmlTableElementBoldAddin.Replace(this.htmlCanaryValue, value).Replace(this.htmlCanaryColSpan, columnSpan.ToString(CultureInfo.CurrentCulture))
                        .Replace(this.htmlCanaryFontSizeInPoints, "10").Replace(this.htmlCanaryWidth, width.ToString(CultureInfo.CurrentCulture)));
            }
            else
            {
                this.fileWriter.WriteLine(
                    this.htmlTableElementAddin.Replace(this.htmlCanaryValue, value).Replace(this.htmlCanaryColSpan, columnSpan.ToString(CultureInfo.CurrentCulture))
                        .Replace(this.htmlCanaryFontSizeInPoints, "10").Replace(this.htmlCanaryWidth, width.ToString(CultureInfo.CurrentCulture)));
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1814:PreferJaggedArraysOverMultidimensional", MessageId = "0#", Justification = "Specifically using a multi-dimensional array.")]
        private static bool InRowMergeRange(int[,] merges, int row, int column)
        {
            new { merges }.Must().NotBeNull();

            // only works for column spans, not row spans
            bool ret = false;
            for (var i = 0; i < merges.GetLength(0); i++)
            {
                // returns false if the cell is the endpoint of the merge range.
                // returns true if the cell is otherwise int he range, start point inclusive
                if (row == merges[i, 0] && column >= merges[i, 1] && column < merges[i, 3])
                {
                    ret = true;
                    break;
                }
            }

            return ret;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Justification = "Disposed correctly.")]
        private void Init()
        {
            this.fileWriter = TextWriter.Synchronized(new StreamWriter(this.fileName));
            this.fileWriter.WriteLine(this.htmlHeaderAddin);
        }

        /// <inheritdoc cref="IDocumentGenerator" />
        public void Close()
        {
            lock (this.syncClose)
            {
                if (!this.closed)
                {
                    this.closed = true;
                    this.fileWriter.WriteLine(this.htmlFooterAddin);
                    this.fileWriter.Close();
                }
            }
        }

        /// <inheritdoc cref="IDisposable" />
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1063:ImplementIDisposableCorrectly", Justification = "Disposed correctly.")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1816:CallGCSuppressFinalizeCorrectly", Justification = "Disposed correctly.")]
        public void Dispose()
        {
            this.Close();
        }

        /// <inheritdoc cref="IDocumentGenerator" />
        public void Indent()
        {
            this.margin += this.marginIncrement;
        }

        /// <inheritdoc cref="IDocumentGenerator" />
        public void Undent()
        {
            this.margin -= this.marginIncrement;
        }

        private string htmlHeaderAddin =
            @"
            <html xmlns:o='urn:schemas-microsoft-com:office:office'
            xmlns:w='urn:schemas-microsoft-com:office:word'
            xmlns='http://www.w3.org/TR/REC-html40'>

            <head>
            <meta http-equiv=Content-Type content='text/html; charset=windows-1252'>
            <meta name=ProgId content=Word.Document>
            <meta name=Generator content='Microsoft Word 11'>
            <meta name=Originator content='Microsoft Word 11'>
            <link rel=File-List href='billing-Documentation_files/filelist.xml'>
            <title>dollar-bills-yall</title>
            <!--[if gte mso 9]><xml>
             <o:DocumentProperties>
              <o:Company>aQuantive, Inc.</o:Company>
              <o:Version>11.6408</o:Version>
             </o:DocumentProperties>
            </xml><![endif]--><!--[if gte mso 9]><xml>
             <w:WordDocument>
              <w:View>Print</w:View>
              <w:Zoom>BestFit</w:Zoom>
              <w:SpellingState>Clean</w:SpellingState>
              <w:GrammarState>Clean</w:GrammarState>
              <w:PunctuationKerning/>
              <w:ValidateAgainstSchemas/>
              <w:SaveIfXMLInvalid>false</w:SaveIfXMLInvalid>
              <w:IgnoreMixedContent>false</w:IgnoreMixedContent>
              <w:AlwaysShowPlaceholderText>false</w:AlwaysShowPlaceholderText>
              <w:Compatibility>
               <w:BreakWrappedTables/>
               <w:SnapToGridInCell/>
               <w:WrapTextWithPunct/>
               <w:UseAsianBreakRules/>
               <w:DontGrowAutofit/>
              </w:Compatibility>
              <w:BrowserLevel>MicrosoftInternetExplorer4</w:BrowserLevel>
             </w:WordDocument>
            </xml><![endif]--><!--[if gte mso 9]><xml>
             <w:LatentStyles DefLockedState='false' LatentStyleCount='156'>
             </w:LatentStyles>
            </xml><![endif]-->
            <style>
            <!--
             /* Style Definitions */
             p.MsoNormal, li.MsoNormal, div.MsoNormal
	            {mso-style-parent:'';
	            _margin:0in;
	            _margin-bottom:.0001pt;
	            mso-pagination:widow-orphan;
	            font-size:[[CanaryFontSizeInPoints]]pt;
	            font-family:'Times New Roman';
	            mso-fareast-font-family:'Times New Roman';}
            span.SpellE
	            {mso-style-name:'';
	            mso-spl-e:yes;}
            span.GramE
	            {mso-style-name:'';
	            mso-gram-e:yes;}
            @page Section1
	            {size:8.5in 11.0in;
	            _margin:0.75in 0.75in 0.75in 0.75in;
	            mso-header-_margin:.5in;
	            mso-footer-_margin:.5in;
	            mso-paper-source:0;}
            div.Section1
	            {page:Section1;}
            -->
            </style>
            <!--[if gte mso 10]>
            <style>
             /* Style Definitions */
             table.MsoNormalTable
	            {mso-style-name:'Table Normal';
	            mso-tstyle-rowband-size:0;
	            mso-tstyle-colband-size:0;
	            mso-style-noshow:yes;
	            mso-style-parent:'';
	            mso-padding-alt:0in 5.4pt 0in 5.4pt;
	            mso-para-_margin:0in;
	            mso-para-_margin-bottom:.0001pt;
	            mso-pagination:widow-orphan;
	            font-size:10.0pt;
	            font-family:'Times New Roman';
	            mso-ansi-language:#0400;
	            mso-fareast-language:#0400;
	            mso-bidi-language:#0400;}
            </style>
            <![endif]-->
            </head>

            <body lang=EN-US style='tab-interval:.5in'>
            ";

        private string htmlFooterAddin =
            @"
            </body>
            </html>
            ";

        private string htmlParagraphBoldAddin =
            @"
            <p class=MsoNormal align=[[CanaryAlign]] style='text-align:[[CanaryAlign]];_margin-left:[[CanaryMarginInPoints]]pt'><b style='mso-bidi-font-weight:normal'><span
            style='font-size:[[CanaryFontSizeInPoints]]pt;mso-bidi-font-size:[[CanaryFontSizeInPoints]]pt'><o:p>[[CanaryValue]]</o:p></span></b></p>
            ";

        private string htmlParagraphAddin =
            @"
            <p class=MsoNormal style='_margin-left:[[CanaryMarginInPoints]]pt'><span
            style='font-size:[[CanaryFontSizeInPoints]]pt;mso-bidi-font-size:[[CanaryFontSizeInPoints]]pt'><o:p>[[CanaryValue]]</o:p></span></p>
            ";

        private string htmlTableHeaderAddin =
            @"
            <table class=MsoNormalTable border=1 cellspacing=0 cellpadding=0
             style='_margin-left:[[CanaryMarginInPoints]]pt;border-collapse:collapse;mso-table-layout-alt:fixed;mso-padding-alt:
             0in 5.4pt 0in 5.4pt'>
            ";

        private string htmlTableFooterAddin =
            @"
            </table>
            ";

        private string htmlTableFirstRowHeaderAddin =
            @"
            <tr style='mso-yfti-irow:0;mso-yfti-firstrow:yes'>
            ";

        private string htmlTableRowHeaderAddin =
            @"
            <tr style='mso-yfti-irow:[[CanaryRowNumber]]'>
            ";

        private string htmlTableRowFooterAddin =
            @"
            </tr>
            ";

        private string htmlTableElementAddin =
            @"
            <td colspan='[[CanaryColSpan]]' valign=top style='width:[[CanaryWidth]]pt;padding:0in 5.4pt 0in 5.4pt'>
            <p class=MsoNormal><span
            style='font-size:10.0pt;mso-bidi-font-size:[[CanaryFontSizeInPoints]]pt'>[[CanaryValue]]<o:p></o:p></span></p>
            </td>
            ";

        private string htmlTableElementBoldAddin =
            @"
            <td colspan='[[CanaryColSpan]]' valign=top style='width:[[CanaryWidth]]pt;padding:0in 5.4pt 0in 5.4pt'>
            <p class=MsoNormal><b style='mso-bidi-font-weight:normal'><span
            style='font-size:[[CanaryFontSizeInPoints]]pt;mso-bidi-font-size:[[CanaryFontSizeInPoints]]pt'>[[CanaryValue]]<o:p></o:p></span></b></p>
            </td>
            ";

        private string htmlCanaryRowNumber = @"[[CanaryRowNumber]]";
        private string htmlCanaryMarginInPoints = @"[[CanaryMarginInPoints]]";
        private string htmlCanaryValue = @"[[CanaryValue]]";
        private string htmlCanaryColSpan = @"[[CanaryColSpan]]";
        private string htmlCanaryFontSizeInPoints = @"[[CanaryFontSizeInPoints]]";
        private string htmlCanaryAlign = @"[[CanaryAlign]]";
        private string htmlCanaryWidth = @"[[CanaryWidth]]";
    }
}