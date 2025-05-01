import { Component, Input } from '@angular/core';
import { EventDetails } from '../../models/event-details.model';
import { CommonModule } from '@angular/common';
import { NzAvatarModule } from 'ng-zorro-antd/avatar';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzIconModule } from 'ng-zorro-antd/icon';

@Component({
  selector: 'app-event-details',
  standalone: true,
  imports: [
    CommonModule,
    NzAvatarModule,
    NzButtonModule,
    NzIconModule
  ],
  template: `
    <div class="sticky top-8 space-y-6">
      <!-- Event Info Card -->
      <div class="bg-white/90 backdrop-blur-sm rounded-2xl shadow-2xl p-6 animate-slide-up-delay border border-white/20">
        <h3 class="text-xl font-bold text-gray-800 mb-4">About this event</h3>
        <div class="space-y-5">
          <!-- Event Description -->
          <p class="text-gray-600">{{event?.description}}</p>
          
          <!-- Society Information - Moved Below -->
          <div class="flex items-center pt-4 border-t border-gray-100">
            <nz-avatar 
              [nzSrc]="event?.societyLogo || ''" 
              [nzText]="getSocietyInitials(event?.societyName || 'Event Organizer')"
              [nzSize]="44"
              nzShape="circle"
              class="flex-shrink-0 mr-3 bg-blue-600 text-white">
            </nz-avatar>
            <div>
              <h4 class="font-medium text-gray-800">{{event?.societyName || 'Event Organizer'}}</h4>
            </div>
          </div>
        </div>
      </div>

      <!-- Contact Card -->
      <div class="bg-white/90 backdrop-blur-sm rounded-2xl shadow-2xl p-6 animate-slide-up-delay-2 border border-white/20">
        <h3 class="text-xl font-bold text-gray-800 mb-4">Need Help?</h3>
        <p class="text-gray-600 mb-4">If you have any questions about the event, feel free to contact us.</p>
        <a href="mailto:{{event?.eventManagerEmail || 'tsp@ju.edu.jo'}}" nz-button nzType="default" 
          class="flex items-center justify-center w-full h-12 rounded-lg border-blue-200 text-blue-600 hover:border-blue-400 hover:text-blue-700 transition-colors duration-300">
          <i nz-icon nzType="mail" class="mr-2"></i>
          Contact Event Organizers
        </a>
      </div>
    </div>
  `
})
export class EventDetailsComponent {
  @Input() event: EventDetails | undefined;
  
  /**
   * Get the initials from the society name (first letter of first and second word)
   * @param societyName The society name to extract initials from
   * @returns The initials (up to 2 characters)
   */
  getSocietyInitials(societyName: string): string {
    if (!societyName) return 'EO'; // Default to "EO" for "Event Organizer"
    
    // Split the name into words
    const words = societyName.split(' ').filter(word => word.length > 0);
    
    if (words.length === 0) return 'EO';
    
    // Get first letter of first word
    const firstInitial = words[0][0].toUpperCase();
    
    // If there's a second word, get its first letter too
    if (words.length > 1) {
      const secondInitial = words[1][0].toUpperCase();
      return firstInitial + secondInitial;
    }
    
    // If only one word, return the first letter only
    return firstInitial;
  }
} 