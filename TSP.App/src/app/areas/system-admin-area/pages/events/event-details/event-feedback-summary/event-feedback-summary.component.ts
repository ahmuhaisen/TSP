import { Component, OnInit, inject } from '@angular/core';
import { ActivatedRoute, RouterModule } from '@angular/router';
import { CommonModule, NgClass } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { NzCardModule } from 'ng-zorro-antd/card';
import { NzDividerModule } from 'ng-zorro-antd/divider';
import { NzIconModule } from 'ng-zorro-antd/icon';
import { NzRateModule } from 'ng-zorro-antd/rate';
import { NzStatisticModule } from 'ng-zorro-antd/statistic';
import { NzTagModule } from 'ng-zorro-antd/tag';
import { NzGridModule } from 'ng-zorro-antd/grid';
import { NzTypographyModule } from 'ng-zorro-antd/typography';
import { NzListModule } from 'ng-zorro-antd/list';
import { NzSkeletonModule } from 'ng-zorro-antd/skeleton';
import { NzEmptyModule } from 'ng-zorro-antd/empty';
import { NzSpinModule } from 'ng-zorro-antd/spin';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { EventFeedbackService, EventFeedbackSummary } from '../../../../../public-forms/event-feedback/event-feedback.service';
import { NzMessageService } from 'ng-zorro-antd/message';
import { catchError, finalize, of } from 'rxjs';

interface EventFeedback {
  rating: number;
  notes: string;
  submittedAt: string;
}

@Component({
  selector: 'app-event-feedback-summary',
  standalone: true,
  imports: [
    NgClass,
    CommonModule,
    FormsModule,
    RouterModule,
    NzCardModule,
    NzDividerModule,
    NzIconModule,
    NzRateModule,
    NzStatisticModule,
    NzTagModule,
    NzGridModule,
    NzTypographyModule,
    NzListModule,
    NzSkeletonModule,
    NzEmptyModule,
    NzSpinModule,
    NzButtonModule
  ],
  templateUrl: './event-feedback-summary.component.html',
  styleUrl: './event-feedback-summary.component.css'
})
export class EventFeedbackSummaryComponent implements OnInit {
  route = inject(ActivatedRoute);
  feedbackService = inject(EventFeedbackService);
  messageService = inject(NzMessageService);
  
  eventId: string = '';
  feedbackData: EventFeedbackSummary | null = null;
  isLoading: boolean = true;
  
  displayLimit: number = 5;
  showAllFeedbacks: boolean = false;
  
  get displayedFeedbacks(): EventFeedback[] {
    if (!this.feedbackData) return [];
    return this.showAllFeedbacks 
      ? this.feedbackData.feedbacks
      : this.feedbackData.feedbacks.slice(0, this.displayLimit);
  }
  
  get hasMoreFeedbacks(): boolean {
    return (this.feedbackData?.summary?.totalResponses || 0) > this.displayLimit;
  }

  get totalFeedbackCount(): number {
    return this.feedbackData?.summary?.totalResponses || 0;
  }

  toggleFeedbacksDisplay(): void {
    this.showAllFeedbacks = !this.showAllFeedbacks;
  }
  
  private fetchFeedbackData(eventId: string): void {
    this.isLoading = true;
    
    this.feedbackService.getEventFeedbackSummary(eventId)
      .pipe(
        catchError(error => {
          console.error('Error fetching feedback summary:', error);
          this.messageService.error('Could not load feedback data. Please try again later.');
          return of(null);
        }),
        finalize(() => {
          this.isLoading = false;
        })
      )
      .subscribe(data => {
        if (data) {
          this.feedbackData = data;
        }
      });
  }

  getSentimentColor(sentiment: string): string {
    switch(sentiment) {
      case 'Positive': return 'success';
      case 'Negative': return 'error';
      case 'Mixed': return 'warning';
      default: return 'default';
    }
  }

  ngOnInit(): void {
    this.eventId = this.route.snapshot.paramMap.get('id') || '';
    if (this.eventId) {
      this.fetchFeedbackData(this.eventId);
    } else {
      this.isLoading = false;
      this.messageService.warning('No event ID provided.');
    }
  }
}
