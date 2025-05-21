import { Component } from '@angular/core';
import { SharedFooterComponent } from '../../../components/shared-footer.component';
import { SharedHeaderComponent } from '../../../components/shared-header.component';

@Component({
  selector: 'app-privacy-policy',
  standalone: true,
  imports: [SharedFooterComponent, SharedHeaderComponent],
  template: `
    <app-shared-header [showNavLinks]="false"/>

    <div class="min-h-screen bg-gray-50 pt-32 pb-16">
      <div class="container mx-auto px-4 sm:px-6 lg:px-8">
        <div class="max-w-3xl mx-auto bg-white rounded-lg shadow-md p-8">
          <h1 class="text-3xl font-bold text-gray-900 mb-8">Privacy Policy</h1>
          
          <div class="space-y-6 text-gray-600">
            <section>
              <h2 class="text-xl font-semibold text-gray-800 mb-3">1. Information We Collect</h2>
              <p class="mb-4">We collect information that you provide directly to us, including:</p>
              <ul class="list-disc pl-6 mb-4">
                <li>Name and contact information</li>
                <li>Student/Faculty ID</li>
                <li>Academic information</li>
                <li>Society membership details</li>
                <li>Event participation records</li>
              </ul>
            </section>

            <section>
              <h2 class="text-xl font-semibold text-gray-800 mb-3">2. How We Use Your Information</h2>
              <p class="mb-4">We use the information we collect to:</p>
              <ul class="list-disc pl-6 mb-4">
                <li>Manage society memberships</li>
                <li>Process event registrations</li>
                <li>Send notifications about society activities</li>
                <li>Generate reports and analytics</li>
                <li>Improve our services</li>
              </ul>
            </section>

            <section>
              <h2 class="text-xl font-semibold text-gray-800 mb-3">3. Information Sharing</h2>
              <p>We do not sell or share your personal information with third parties except as necessary to:</p>
              <ul class="list-disc pl-6 mb-4">
                <li>Comply with legal obligations</li>
                <li>Protect our rights and safety</li>
                <li>Facilitate society operations with your consent</li>
              </ul>
            </section>

            <section>
              <h2 class="text-xl font-semibold text-gray-800 mb-3">4. Data Security</h2>
              <p>We implement appropriate security measures to protect your personal information, including:</p>
              <ul class="list-disc pl-6 mb-4">
                <li>Encryption of data in transit and at rest</li>
                <li>Regular security assessments</li>
                <li>Access controls and authentication</li>
                <li>Secure data backup procedures</li>
              </ul>
            </section>

            <section>
              <h2 class="text-xl font-semibold text-gray-800 mb-3">5. Your Rights</h2>
              <p>You have the right to:</p>
              <ul class="list-disc pl-6 mb-4">
                <li>Access your personal information</li>
                <li>Correct inaccurate information</li>
                <li>Request deletion of your information</li>
                <li>Opt-out of communications</li>
              </ul>
            </section>

            <section>
              <h2 class="text-xl font-semibold text-gray-800 mb-3">6. Contact Us</h2>
              <p>If you have questions about this Privacy Policy, please contact us at:</p>
              <p class="mt-2">Email: privacy&#64;societiesportal.com</p>
            </section>

            <section>
              <h2 class="text-xl font-semibold text-gray-800 mb-3">7. Updates to This Policy</h2>
              <p>We may update this Privacy Policy from time to time. The updated version will be indicated by an updated "Last revised" date and the updated version will be effective as soon as it is accessible.</p>
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
export class PrivacyPolicyComponent {} 