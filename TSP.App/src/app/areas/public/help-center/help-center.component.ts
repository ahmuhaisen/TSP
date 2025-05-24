import { Component } from '@angular/core';
import { SharedFooterComponent } from "../../../components/shared-footer.component";
import { SharedHeaderComponent } from '../../../components/shared-header.component';

@Component({
  selector: 'app-help-center',
  standalone: true,
  imports: [SharedFooterComponent, SharedHeaderComponent],
  template: `
    <app-shared-header [showNavLinks]="false"/>
    
    <div class="min-h-screen bg-gray-50 pt-32 pb-16">
      <div class="container mx-auto px-4 sm:px-6 lg:px-8">
        <div class="max-w-3xl mx-auto bg-white rounded-lg shadow-md p-8">
          <h1 class="text-3xl font-bold text-gray-900 mb-8">Help Center</h1>
          
          <div class="space-y-6 text-gray-600">
            <section>
              <h2 class="text-xl font-semibold text-gray-800 mb-3">Getting Started</h2>
              <p class="mb-4">Welcome to The Societies Portal Help Center. Here you'll find answers to common questions and guidance on using our platform effectively.</p>
            </section>

            <section>
              <h2 class="text-xl font-semibold text-gray-800 mb-3">Account Management</h2>
              <div class="mb-4">
                <h3 class="font-medium text-gray-700 mb-2">How do I create an account?</h3>
                <p class="mb-4">Click the "Sign Up" button on the homepage and follow the registration process. You'll need to provide your university email address and create a password.</p>
                
                <h3 class="font-medium text-gray-700 mb-2">How do I reset my password?</h3>
                <p class="mb-4">Click "Forgot Password" on the login page and follow the instructions sent to your email address.</p>
              </div>
            </section>

            <section>
              <h2 class="text-xl font-semibold text-gray-800 mb-3">Society Management</h2>
              <div class="mb-4">
                <h3 class="font-medium text-gray-700 mb-2">How do I join a society?</h3>
                <p class="mb-4">Browse available societies on the platform, select the one you're interested in, and click "Join Society". Some societies may require approval from their administrators.</p>
                
                <h3 class="font-medium text-gray-700 mb-2">How do I create a new society?</h3>
                <p class="mb-4">Navigate to the "Create Society" section, fill in the required information, and submit for approval. You'll need to provide society details, purpose, and initial member information.</p>
              </div>
            </section>

            <section>
              <h2 class="text-xl font-semibold text-gray-800 mb-3">Events and Activities</h2>
              <div class="mb-4">
                <h3 class="font-medium text-gray-700 mb-2">How do I create an event?</h3>
                <p class="mb-4">As a society administrator, go to your society's dashboard and select "Create Event". Fill in the event details, date, time, and location.</p>
                
                <h3 class="font-medium text-gray-700 mb-2">How do I RSVP to an event?</h3>
                <p class="mb-4">Find the event in the events section, click on it to view details, and select "RSVP" to confirm your attendance.</p>
              </div>
            </section>

            <section>
              <h2 class="text-xl font-semibold text-gray-800 mb-3">Technical Support</h2>
              <div class="mb-4">
                <h3 class="font-medium text-gray-700 mb-2">What should I do if I encounter technical issues?</h3>
                <p class="mb-4">Try clearing your browser cache and refreshing the page. If the issue persists, contact our support team at 
                  <a href="mailto:{{helpCenterEmail}}" class="text-indigo-600 hover:underline">{{helpCenterEmail}}</a>.
                </p>
                
                <h3 class="font-medium text-gray-700 mb-2">Which browsers are supported?</h3>
                <p class="mb-4">The platform works best on the latest versions of Chrome, Firefox, Safari, and Edge.</p>
              </div>
            </section>

            <section>
              <h2 class="text-xl font-semibold text-gray-800 mb-3">Contact Us</h2>
              <p class="mb-4">If you need further assistance, please contact our support team:</p>
              <ul class="list-disc pl-6 mb-4">
                <li>Email: 
                  <a href="mailto:{{helpCenterEmail}}" class="text-indigo-600 hover:underline">{{helpCenterEmail}}</a>
                </li>
                <li>Response Time: Within 24-48 hours</li>
                <li>Office Hours: Sunday to Thursday, 8 AM - 3 PM</li>
              </ul>
            </section>
          </div>

          <div class="mt-8 pt-6 border-t border-gray-200">
            <p class="text-xs text-gray-500">Last updated: May 2025</p>
          </div>
        </div>
      </div>
    </div>
    <app-shared-footer />
  `
})
export class HelpCenterComponent {
  helpCenterEmail = 'support@tsp.app'
} 