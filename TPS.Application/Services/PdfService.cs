using System;
using System.IO;
using iText.Kernel.Colors;
using iText.Kernel.Events;
using iText.Kernel.Font;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using iText.IO.Image;
using iText.Layout.Element;
using iText.Layout.Properties;
using TPS.Application.Abstractions;
using iText.IO.Font.Constants;
using iText.Layout.Borders;


namespace TPS.Application.Services;


public class PdfService : IPdfService
{
    public Task<byte[]> GeneratePdf(string content)
    {
        using var memoryStream = new MemoryStream();

        var settings = new PdfSettings
        {
            OutputStream = memoryStream,
            Title = "Monthly Financial Report",
            LogoPath = "C:\\logos\\company-logo.png",
            PortalName = "Financial Services Inc.",
            Copyright = $"© {DateTime.Now.Year} Financial Services Inc. - Confidential",
            TitleFont = PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD),
            BodyFont = PdfFontFactory.CreateFont(StandardFonts.HELVETICA),
            HeaderColor = new DeviceRgb(63, 81, 181),
            FooterColor = new DeviceRgb(117, 117, 117)
        };

        using (var generator = new PdfDocumentGenerator(settings))
        {
            generator.Generate(); // Add styled title/date

            generator.AddParagraph("Executive Summary", p =>
            {
                p.SetBorderBottom(new SolidBorder(settings.HeaderColor, 1))
                .SetMarginBottom(10);
            }, 16, TextAlignment.LEFT);


            // Add professional table
            // if (records?.Any() == true)
            // {
            //     generator.AddTable(
            //         records,
            //         "Date", "Transaction ID", "Description", "Amount"
            //     );
            // }

            // Add disclaimer
            generator.AddParagraph("Executive Summary 2", p =>
            {
                p.SetBorderBottom(new SolidBorder(settings.HeaderColor, 1))
                .SetMarginBottom(10);
            }, 16, TextAlignment.LEFT);
        }

        return Task.FromResult(memoryStream.ToArray());
    }
}


public class PdfSettings
{
    public string OutputPath { get; set; } = "output.pdf";
    public Stream OutputStream { get; set; }
    public string Title { get; set; } = "Default Document Title";
    public string LogoPath { get; set; } = "logo.png";
    public string PortalName { get; set; } = "The Societies Portal";
    public string Copyright { get; set; } = "© 2024 The Societies Portal. All rights reserved.";
    public Color HeaderColor { get; set; } = new DeviceRgb(0, 0, 0);
    public Color FooterColor { get; set; } = new DeviceRgb(100, 100, 100);
    public PdfFont TitleFont { get; set; } = PdfFontFactory.CreateFont();

    public PdfFont BodyFont { get; set; } = PdfFontFactory.CreateFont();


    // Professional color palette
    public Color PrimaryColor { get; set; } = new DeviceRgb(63, 81, 181);
    public Color SecondaryColor { get; set; } = new DeviceRgb(117, 117, 117);
    public Color AccentColor { get; set; } = new DeviceRgb(255, 152, 0);
}

public class PdfDocumentGenerator : IDisposable
{
    private readonly PdfSettings _settings;
    private readonly PdfWriter _writer;
    private readonly PdfDocument _pdfDocument;
    private readonly Document _document;

    public PdfDocumentGenerator(PdfSettings settings)
    {
        _settings = settings ?? throw new ArgumentNullException(nameof(settings));

        // Use stream if provided, otherwise use file path
        _writer = _settings.OutputStream != null
            ? new PdfWriter(_settings.OutputStream)
            : new PdfWriter(_settings.OutputPath);

        _pdfDocument = new PdfDocument(_writer);
        _document = new Document(_pdfDocument, PageSize.A4);

        var eventHandler = new TemplatePageEvent(_settings);
        _pdfDocument.AddEventHandler(PdfDocumentEvent.START_PAGE, eventHandler);
        _pdfDocument.AddEventHandler(PdfDocumentEvent.END_PAGE, eventHandler);
    }

    // Modified AddParagraph to allow pre-add configuration
    public Paragraph AddParagraph(string text, Action<Paragraph> configure, float fontSize = 12, TextAlignment alignment = TextAlignment.LEFT)
    {
        var paragraph = new Paragraph(text)
            .SetFont(_settings.BodyFont)
            .SetFontSize(fontSize)
            .SetTextAlignment(alignment);

        configure?.Invoke(paragraph); // Apply custom styling
        _document.Add(paragraph);
        return paragraph;
    }

    public void AddTable<T>(IEnumerable<T> data, params string[] columnHeaders)
    {
        var table = new Table(UnitValue.CreatePercentArray(columnHeaders.Length))
            .UseAllAvailableWidth()
            .SetMarginTop(20)
            .SetMarginBottom(20);

        // Header row
        foreach (var header in columnHeaders)
        {
            table.AddHeaderCell(new Cell()
                .SetBackgroundColor(new DeviceRgb(63, 81, 181))
                .SetFontColor(ColorConstants.WHITE) // Fixed
                .SetTextAlignment(TextAlignment.CENTER)
                .Add(new Paragraph(header).SetFont(_settings.BodyFont).SetBold()));
        }

        // Data rows with alternating colors
        int rowIndex = 0;
        var properties = typeof(T).GetProperties();
        foreach (var item in data)
        {
            foreach (var prop in properties)
            {
                var cellColor = rowIndex % 2 == 0
                    ? new DeviceRgb(245, 245, 245)
                    : ColorConstants.WHITE; // Fixed

                table.AddCell(new Cell()
                    .SetBackgroundColor(cellColor)
                    .SetTextAlignment(TextAlignment.CENTER)
                    .Add(new Paragraph(prop.GetValue(item)?.ToString() ?? "")
                        .SetFont(_settings.BodyFont)));
            }
            rowIndex++;
        }

        _document.Add(table);
    }


    public void Generate()
    {
        // Add title with enhanced styling
        var title = new Paragraph(_settings.Title)
            .SetFont(_settings.TitleFont)
            .SetFontSize(24)
            .SetTextAlignment(TextAlignment.CENTER)
            .SetFontColor(new DeviceRgb(33, 33, 33))
            .SetMarginTop(40)
            .SetMarginBottom(15)
            .SetBorderBottom(new SolidBorder(new DeviceRgb(63, 81, 181), 2))
            .SetPaddingBottom(10);

        _document.Add(title);

        // Add date with subtle styling
        var date = new Paragraph(DateTime.Now.ToString("dd MMMM yyyy"))
            .SetFont(_settings.BodyFont)
            .SetFontSize(11)
            .SetTextAlignment(TextAlignment.CENTER)
            .SetFontColor(new DeviceRgb(117, 117, 117))
            .SetMarginBottom(30);

        _document.Add(date);
    }

    public void Dispose()
    {
        _document?.Close();
        _pdfDocument?.Close();
        _writer?.Close();
        GC.SuppressFinalize(this);
    }
}

internal class TemplatePageEvent : IEventHandler
{
    private readonly PdfSettings _settings;
    private readonly float _headerHeight = 100f;
    private readonly float _footerHeight = 50f;

    public TemplatePageEvent(PdfSettings settings)
    {
        _settings = settings;
    }

    public void HandleEvent(Event @event)
    {
        PdfDocumentEvent docEvent = (PdfDocumentEvent)@event;
        PdfPage page = docEvent.GetPage();
        Rectangle pageSize = page.GetPageSize();
        PdfCanvas pdfCanvas = new PdfCanvas(page.NewContentStreamBefore(), page.GetResources(), docEvent.GetDocument());

        if (docEvent.GetEventType() == PdfDocumentEvent.START_PAGE)
        {
            DrawHeader(pdfCanvas, pageSize);
        }
        else if (docEvent.GetEventType() == PdfDocumentEvent.END_PAGE)
        {
            DrawFooter(pdfCanvas, pageSize, docEvent.GetDocument().GetPageNumber(page));
        }

        pdfCanvas.Release();
    }

    private void DrawHeader(PdfCanvas canvas, Rectangle pageSize)
    {
        try
        {
            // Logo positioning (centered vertically in header area)
            if (!string.IsNullOrEmpty(_settings.LogoPath) && File.Exists(_settings.LogoPath))
            {
                ImageData logoData = ImageDataFactory.Create(_settings.LogoPath);
                float logoX = 50;
                float logoY = pageSize.GetTop() - 60; // 60px from top
                canvas.AddImageAt(logoData, logoX, logoY, false);
            }
        }
        catch (Exception ex)
        {
            // Implement proper logging here
            Console.WriteLine($"Header logo error: {ex.Message}");
        }

        // Professional portal name styling
        canvas.BeginText()
            .SetFontAndSize(_settings.TitleFont, 18)
            .SetColor(new DeviceRgb(63, 81, 181), true) // Professional blue color
            .SetTextRenderingMode(PdfCanvasConstants.TextRenderingMode.FILL_STROKE)
            .SetLineWidth(0.5f)
            .MoveText(150, pageSize.GetTop() - 45) // Align with logo
            .ShowText(_settings.PortalName)
            .EndText();
    }

    private void DrawFooter(PdfCanvas canvas, Rectangle pageSize, int pageNumber)
    {
        // Footer background
        canvas.SaveState()
            .SetFillColor(new DeviceRgb(245, 245, 245))
            .Rectangle(0, 0, pageSize.GetWidth(), _footerHeight)
            .Fill()
            .RestoreState();

        // Copyright text
        canvas.BeginText()
            .SetFontAndSize(_settings.BodyFont, 10)
            .SetColor(_settings.FooterColor, true)
            .MoveText(50, 20)
            .ShowText(_settings.Copyright)
            .EndText();

        // Page number with professional styling
        var pageText = $"Page {pageNumber}";
        canvas.BeginText()
            .SetFontAndSize(_settings.BodyFont, 10)
            .SetColor(_settings.PrimaryColor, true)
            .MoveText(pageSize.GetWidth() - 100, 20)
            .ShowText(pageText)
            .EndText();
    }
}