import { Component } from '@angular/core';
import { SharedFooterComponent } from "../../../components/shared-footer.component";
import { SharedHeaderComponent } from '../../../components/shared-header.component';

@Component({
  selector: 'app-terms-of-use',
  standalone: true,
  imports: [SharedFooterComponent, SharedHeaderComponent],
  template: `
    <app-shared-header [showNavLinks]="false"/>
    
    <div class="min-h-screen bg-gray-50 pt-32 pb-16">
      <div class="container mx-auto px-4 sm:px-6 lg:px-8">
        <div class="max-w-3xl mx-auto bg-white rounded-lg shadow-md p-8">
          <h1 class="text-3xl font-bold text-gray-900 mb-8">Terms of Use</h1>
          
          <div class="space-y-6 text-gray-600">
            <section>
              <h2 class="text-xl font-semibold text-gray-800 mb-3">1. Acceptance of Terms</h2>
              <p class="mb-4">By accessing and using The Societies Portal, you accept and agree to be bound by these Terms of Use and our Privacy Policy.</p>
            </section>

            <section>
              <h2 class="text-xl font-semibold text-gray-800 mb-3">2. User Accounts</h2>
              <p class="mb-4">You are responsible for:</p>
              <ul class="list-disc pl-6 mb-4">
                <li>Maintaining the confidentiality of your account</li>
                <li>All activities that occur under your account</li>
                <li>Notifying us of any unauthorized access</li>
                <li>Ensuring your account information is accurate</li>
              </ul>
            </section>

            <section>
              <h2 class="text-xl font-semibold text-gray-800 mb-3">3. Acceptable Use</h2>
              <p class="mb-4">You agree not to:</p>
              <ul class="list-disc pl-6 mb-4">
                <li>Use the service for any illegal purpose</li>
                <li>Share false or misleading information</li>
                <li>Interfere with the service's operation</li>
                <li>Attempt to gain unauthorized access</li>
                <li>Share your account credentials</li>
              </ul>
            </section>

            <section>
              <h2 class="text-xl font-semibold text-gray-800 mb-3">4. Content and Conduct</h2>
              <p class="mb-4">Users must ensure that all content and conduct:</p>
              <ul class="list-disc pl-6 mb-4">
                <li>Complies with university policies</li>
                <li>Respects intellectual property rights</li>
                <li>Does not harass or discriminate</li>
                <li>Is accurate and truthful</li>
              </ul>
            </section>

            <section>
              <h2 class="text-xl font-semibold text-gray-800 mb-3">5. Termination</h2>
              <p>We reserve the right to:</p>
              <ul class="list-disc pl-6 mb-4">
                <li>Suspend or terminate accounts</li>
                <li>Remove or edit content</li>
                <li>Deny access to the service</li>
              </ul>
            </section>

            <section>
              <h2 class="text-xl font-semibold text-gray-800 mb-3">6. Disclaimers</h2>
              <p class="mb-4">The service is provided "as is" without warranties of any kind, either express or implied.</p>
            </section>

            <section>
              <h2 class="text-xl font-semibold text-gray-800 mb-3">7. Limitation of Liability</h2>
              <p class="mb-4">We shall not be liable for any indirect, incidental, special, consequential, or punitive damages resulting from your use of the service.</p>
            </section>

            <section>
              <h2 class="text-xl font-semibold text-gray-800 mb-3">8. Changes to Terms</h2>
              <p>We may modify these terms at any time. Continued use of the service constitutes acceptance of modified terms.</p>
            </section>
          </div>

          <div class="mt-8 pt-6 border-t border-gray-200">
          <p class="text-xs text-gray-500">Last revised: April 2025</p>
          </div>
        </div>
      </div>
    </div>
    <app-shared-footer />
  `
})
export class TermsOfUseComponent {} 