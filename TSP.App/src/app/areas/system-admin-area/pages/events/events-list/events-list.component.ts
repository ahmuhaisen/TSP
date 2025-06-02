import { Component, ViewChild } from '@angular/core';
import { NzBreadCrumbModule } from 'ng-zorro-antd/breadcrumb';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzDividerModule } from 'ng-zorro-antd/divider';
import { NzIconModule } from 'ng-zorro-antd/icon';
import { RequestsTableComponent } from "./requests-table/requests-table.component";
import { EventsScheduleComponent } from "./events-schedule/events-schedule.component";
import { NzToolTipModule } from 'ng-zorro-antd/tooltip';
import { NzDropDownModule } from 'ng-zorro-antd/dropdown';
import { NzMessageService } from 'ng-zorro-antd/message';
import jsPDF from 'jspdf';
import { DatePipe } from '@angular/common';
import { NzDrawerModule } from 'ng-zorro-antd/drawer';
import { NzTabsModule } from 'ng-zorro-antd/tabs';
import { NzCalendarModule } from 'ng-zorro-antd/calendar';
import { FormsModule } from '@angular/forms';
import { EventSimpleDTO } from '../../../../student-area/api-interfaces/event.types';
import { NzBadgeModule } from 'ng-zorro-antd/badge';
import { NzCardModule } from 'ng-zorro-antd/card';
import { NzEmptyModule } from 'ng-zorro-antd/empty';
import { NzTagModule } from 'ng-zorro-antd/tag';
import { EventsService } from '../../../../student-area/services/events.service'
import { inject } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-events-list',
  imports: [
    NzBreadCrumbModule,
    NzButtonModule,
    NzIconModule,
    NzDividerModule,
    NzToolTipModule,
    NzDropDownModule,
    RequestsTableComponent,
    DatePipe,
    NzDrawerModule,
    NzTabsModule,
    NzCalendarModule,
    FormsModule,
    NzBadgeModule,
    NzCardModule,
    NzEmptyModule,
    NzTagModule,
    CommonModule
  ],
  templateUrl: './events-list.component.html',
  styleUrl: './events-list.component.css',
  providers: [DatePipe]
})
export class EventsListComponent {
  @ViewChild(RequestsTableComponent) requestsTable!: RequestsTableComponent;

  constructor(private messageSvc: NzMessageService, private datePipe: DatePipe, private eventsService: EventsService) {
    this.eventsService.getEventsByMonth().subscribe(data => {
      this.upcomingEvents = data;
    })
  }

  exportToPdf(): void {
    if (!this.requestsTable || !this.requestsTable.eventsRequests || this.requestsTable.eventsRequests.length === 0) {
      this.messageSvc.error('No data available to export');
      return;
    }

    // Create PDF document
    const pdf = new jsPDF({
      orientation: 'landscape',
      unit: 'mm',
      format: 'a4'
    });

    // Set title
    pdf.setFontSize(16);
    pdf.setFont('helvetica', 'bold');
    pdf.text('Events Requests Report', 150, 15, { align: 'center' });

    // Add current date
    pdf.setFontSize(10);
    pdf.setFont('helvetica', 'normal');
    const currentDate = this.datePipe.transform(new Date(), 'yyyy-MM-dd HH:mm:ss');
    pdf.text(`Generated on: ${currentDate || ''}`, 20, 25);

    // Create the table manually (without headers)
    const tableRows = this.requestsTable.eventsRequests.map(request => [
      request.eventName,
      request.eventSociety.societyName,
      this.datePipe.transform(request.startDateTime, 'yyyy-MM-dd HH:mm:ss') || '',
      request.locationString,
      request.approvalStatus
    ]);

    // Define table configuration
    const startX = 20;
    let startY = 30;
    const cellPadding = 3;
    const columnWidths = [50, 50, 50, 50, 30]; // width for each column
    const rowHeight = 10;

    // Draw table rows - start immediately without headers
    pdf.setFont('helvetica', 'normal');
    pdf.setTextColor(0, 0, 0);

    for (let i = 0; i < tableRows.length; i++) {
      // Add alternating row background
      if (i % 2 === 0) {
        pdf.setFillColor(240, 240, 240);
        pdf.rect(
          startX,
          startY,
          columnWidths.reduce((a, b) => a + b, 0),
          rowHeight,
          'F'
        );
      }

      for (let j = 0; j < tableRows[i].length; j++) {
        // Add cell text
        pdf.text(
          String(tableRows[i][j]).substring(0, 25) + (String(tableRows[i][j]).length > 25 ? '...' : ''),
          startX + cellPadding + columnWidths.slice(0, j).reduce((a, b) => a + b, 0),
          startY + rowHeight / 2 + 1
        );
      }

      startY += rowHeight;

      // Add a new page if we're at the bottom
      if (startY > pdf.internal.pageSize.height - 20) {
        pdf.addPage();
        startY = 20;
      }
    }

    // Draw table grid (adjusted to start directly with data rows, no header)
    startY = 30;
    for (let i = 0; i <= tableRows.length; i++) {
      // Horizontal lines
      pdf.line(
        startX,
        startY + i * rowHeight,
        startX + columnWidths.reduce((a, b) => a + b, 0),
        startY + i * rowHeight
      );
    }

    // Vertical lines
    let columnX = startX;
    for (let i = 0; i <= columnWidths.length; i++) {
      pdf.line(
        columnX,
        startY,
        columnX,
        startY + tableRows.length * rowHeight
      );
      if (i < columnWidths.length) {
        columnX += columnWidths[i];
      }
    }

    // Save PDF
    pdf.save('events-requests-report.pdf');
    this.messageSvc.success('PDF exported successfully');
  }

  exportToCsv(): void {
    if (!this.requestsTable || !this.requestsTable.eventsRequests || this.requestsTable.eventsRequests.length === 0) {
      this.messageSvc.error('No data available to export');
      return;
    }

    // Define the headers
    const headers = ['Name', 'Society', 'Date and Time', 'Location', 'Status'];

    // Map data to CSV format
    const csvRows = this.requestsTable.eventsRequests.map(request => [
      request.eventName,
      request.eventSociety.societyName,
      this.datePipe.transform(request.startDateTime, 'yyyy-MM-dd HH:mm:ss') || '',
      request.locationString,
      request.approvalStatus
    ]);

    // Add headers to beginning of rows
    csvRows.unshift(headers);

    // Convert to CSV format
    const csvContent = csvRows.map(row => row.map(cell =>
      // Escape quotes and wrap in quotes if the cell contains commas, quotes, or newlines
      typeof cell === 'string' && (cell.includes(',') || cell.includes('"') || cell.includes('\n'))
        ? `"${cell.replace(/"/g, '""')}"`
        : cell
    ).join(',')).join('\n');

    // Create a Blob from the CSV string
    const blob = new Blob([csvContent], { type: 'text/csv;charset=utf-8;' });

    // Create download link
    const link = document.createElement('a');
    const url = URL.createObjectURL(blob);
    link.setAttribute('href', url);
    link.setAttribute('download', 'events-requests.csv');
    link.style.visibility = 'hidden';

    // Add to document, trigger click and remove
    document.body.appendChild(link);
    link.click();
    document.body.removeChild(link);

    this.messageSvc.success('CSV exported successfully');
  }
  isEventDetailsVisible: boolean = false;

  currentDate: Date = new Date();
  viewType: string = "cards";
  selectedDate: Date | null = null;
  isEventModalVisible = false;
  upcomingEvents: EventSimpleDTO[] = [];
  selectedEvent: any = null;
  setViewType(type: 'calendar' | 'cards'): void {
    this.viewType = type;
  }
  disabledDate = (current: Date): boolean => {
    const now = new Date().getTime();
    const lowerBound = now;
    const upperBound = now + 30 * 24 * 60 * 60 * 1000;
    return current.getTime() < lowerBound || current.getTime() > upperBound;
  };

  openEventModal(date: Date): void {
    this.selectedDate = date;
    this.isEventModalVisible = true;
  }
  getDayEvents(date: Date): any[] {
    if (!date) return [];
    return this.upcomingEvents.filter(event => {
      const eventDate = new Date(event.startTime);
      return eventDate.getDate() === date.getDate() &&
        eventDate.getMonth() === date.getMonth() &&
        eventDate.getFullYear() === date.getFullYear();
    });
  }
  getEventBadgeStatus(eventDate: string): string {
    const today = new Date();
    today.setHours(0, 0, 0, 0);
    const eventDay = new Date(eventDate);
    eventDay.setHours(0, 0, 0, 0);

    if (eventDay.getTime() === today.getTime()) {
      return 'success';  // green for today
    } else if (eventDay < today) {
      return 'warning';  // orange for past events
    } else {
      return 'processing';  // blue for upcoming events
    }
  }
  closeEventDetails(): void {
    this.selectedEvent = null;
    this.isEventDetailsVisible = false;
  }

  ngOnInit() {

  }
}