import { Component } from '@angular/core';
import { interval, Subscription } from 'rxjs';

@Component({
  selector: 'app-coming-soon',
  imports: [],
  template: `
  <main class="h-screen flex justify-center items-center bg-cover bg-center"
    style="background-image: url('https://vojislavd.com/ta-template-demo/assets/img/coming-soon.jpg');">
    <div class="h-fit flex flex-col gap-y-5 items-center px-4 sm:px-6 lg:px-10">

      <div class="text-center space-y-2 text-[#f0f0f0]">
        <h1 class="text-3xl sm:text-4xl lg:text-5xl font-bold text-[#f0f0f0]">{{ title }}</h1>
        <p class="text-lg">Empowering Connections, Enriching Communities 🔥</p>
      </div>

      <div class="py-4">
        <div class="flex flex-wrap gap-4 justify-center items-center text-[#f0f0f0]">
          <div class="border-2 rounded-lg bg-slate-50/25 px-4 py-2 text-center w-fit">
            <div class="font-bold font-mono text-2xl sm:text-3xl">{{ countdown.days }}d</div>
          </div>
          <div class="border-2 rounded-lg bg-slate-50/25 px-4 py-2 text-center w-fit">
            <div class="font-bold font-mono text-2xl sm:text-3xl">{{ countdown.hours }}h</div>
          </div>
          <div class="border-2 rounded-lg bg-slate-50/25 px-4 py-2 text-center w-fit">
            <div class="font-bold font-mono text-2xl sm:text-3xl">{{ countdown.minutes }}m</div>
          </div>
          <div class="border-2 rounded-lg bg-slate-50/25 px-4 py-2 text-center w-fit">
            <div class="font-bold font-mono text-2xl sm:text-3xl">{{ countdown.seconds }}s</div>
          </div>
        </div>
      </div>
    </div>
  </main>
  `,
})
export class ComingSoonComponent {
  title = 'The Societies Portal';

  countdown = {
    days: 0,
    hours: 0,
    minutes: 0,
    seconds: 0
  };

  targetDate = new Date("Jun 1, 2025 00:00:00").getTime();
  private timerSubscription!: Subscription;

  ngOnInit(): void {
    this.timerSubscription = interval(1000).subscribe(() => {
      const now = new Date().getTime();
      const distance = this.targetDate - now;

      if (distance > 0) {
        this.countdown.days = Math.floor(distance / (1000 * 60 * 60 * 24));
        this.countdown.hours = Math.floor((distance % (1000 * 60 * 60 * 24)) / (1000 * 60 * 60));
        this.countdown.minutes = Math.floor((distance % (1000 * 60 * 60)) / (1000 * 60));
        this.countdown.seconds = Math.floor((distance % (1000 * 60)) / 1000);
      } else {
        this.countdown.days = 0;
        this.countdown.hours = 0;
        this.countdown.minutes = 0;
        this.countdown.seconds = 0;
      }
    });
  }

  ngOnDestroy(): void {
    this.timerSubscription.unsubscribe();
  }
}
