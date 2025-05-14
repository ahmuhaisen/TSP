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

interface EventFeedback {
  rating: number;
  notes: string;
  submittedAt: string;
}

interface EventFeedbackSummary {
  event: {
    id: string;
    name: string;
  };
  summary: {
    summaryId: string;
    averageRating: number;
    totalResponses: number;
    sentiment: string;
    topics: string;
    aiSummary: string;
    calculatedAt: string;
  };
  feedbacks: EventFeedback[];
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
  
  // This method will be replaced with actual API call
  private fetchFeedbackData(eventId: string): void {
    // Simulating API call with mock data
    setTimeout(() => {
      this.feedbackData = {
        "event": {
          "id": "e6b7d1b7-5262-417e-92c0-2b4f29fc43ce",
          "name": "GitHub Workshop"
        },
        "summary": {
          "summaryId": "462d6b55-c8e4-441c-9ba1-08dd8e4ca2b4",
          "averageRating": 2.85,
          "totalResponses": 10,
          "sentiment": "Mixed",
          "topics": "Speaker, Organization, Timing",
          "aiSummary": "While students found the workshop to be of interest and appreciated its timing in being on a nice day, there are significant concerns about the quality of the speaker's performance. Additionally, negative feedback highlighted issues with event organization, including an ill-timed start that led to distractions from other attendees.",
          "calculatedAt": "2025-05-08T19:23:15.5166667"
        },
        "feedbacks": [
          {
            "rating": 4,
            "notes": "Great workshop, but the organization is bad, late start, student stalks too much, so i couldnt focus",
            "submittedAt": "2025-05-08T19:27:08.3126124"
          },
          {
            "rating": 2.5,
            "notes": "it was nice",
            "submittedAt": "2025-05-08T19:24:40.7905223"
          },
          {
            "rating": 1.5,
            "notes": "The workshop was nice, but the instructor is not good",
            "submittedAt": "2025-05-08T19:22:39.6388029"
          },
          {
            "rating": 2,
            "notes": "it was a nice workshop, very important topic, but the instructor was not that good.",
            "submittedAt": "2025-05-08T19:16:48.2583622"
          },
          {
            "rating": 2.5,
            "notes": "The event was nice, but the speaker was not that good, please dont let him give a lecture at out beloved university again, thanks.",
            "submittedAt": "2025-05-08T19:13:50.202507"
          }
        ]
      };
      this.isLoading = false;
    }, 500);
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
    this.fetchFeedbackData(this.eventId);
  }
}
