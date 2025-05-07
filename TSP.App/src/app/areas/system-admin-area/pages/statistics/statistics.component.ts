import { Component, OnInit, signal, ViewChild } from '@angular/core';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzIconModule } from 'ng-zorro-antd/icon';
import { ContainerBlockComponent } from "../../../../components/container-block.component";
import { BaseChartDirective } from 'ng2-charts';
import { ChartConfiguration, ChartOptions, ChartData } from 'chart.js';
import { NzDividerModule } from 'ng-zorro-antd/divider';
import { NzEmptyModule } from 'ng-zorro-antd/empty';
import { RouterLink } from '@angular/router';
import { NgClass } from '@angular/common';
import { StatisticsService, SocietyData } from '../../services/statistics.service';
import { NzAvatarModule } from 'ng-zorro-antd/avatar';
import { TruncatePipe } from "../../../../common/pipes/truncate.pipe";

@Component({
  selector: 'app-statistics',
  imports: [
    NgClass,
    RouterLink,
    NzIconModule,
    NzButtonModule,
    NzDividerModule,
    NzEmptyModule,
    NzAvatarModule,
    BaseChartDirective,
    ContainerBlockComponent,
  ],
  templateUrl: './statistics.component.html',
  styleUrl: './statistics.component.css'
})
export class StatisticsComponent implements OnInit {
  // Loading states
  isLineChartEmpty = false;
  isPieChartEmpty = false;
  isBarChartEmpty = false;
  isTopSocietiesEmpty = false;

  constructor(private statisticsService: StatisticsService) { }

  ngOnInit() {
    this.loadEventsByMonth();
    this.loadTopSocietiesByMembers();
    this.loadTopEventsByAttendance();
    this.loadTopSocieties();
  }

  private loadEventsByMonth() {
    this.statisticsService.getEventsByMonth(6).subscribe(data => {
      this.lineChartDatasets[0].data = data.map(item => item.eventCount);
      this.isLineChartEmpty = data.length === 0 || data.every(item => item.eventCount === 0);
      this.lineChartLabels = data.map(item => item.month);

      this.chart?.update();
    });
  }

  private loadTopSocietiesByMembers() {
    this.statisticsService.getTopSocietiesByMembers(4).subscribe(data => {
      console.log(data);

      this.pieChartLabels = data.map(item => item.name);
      this.pieChartDatasets[0].data = data.map(item => item.count);
      this.isPieChartEmpty = data.length === 0 || data.every(item => item.count === 0);

      this.chart?.update();
    });
  }

  private loadTopEventsByAttendance() {
    this.statisticsService.getTopEventsByAttendance(5).subscribe(data => {
      this.isBarChartEmpty = data.length === 0 || data.every(item => item.count === 0);

      this.barChartData = {
        labels: data.map(item => item.eventName),
        datasets: [
          {
            data: data.map(item => item.count),
            label: 'No of attendees',
          }
        ]
      };

      this.chart?.update();
    });
  }

  private loadTopSocieties() {
    this.statisticsService.getTopSocieties(3).subscribe(data => {
      this.otherSocieties = data;
      this.isTopSocietiesEmpty = data.length === 0;
    });
  }

  @ViewChild(BaseChartDirective) chart?: BaseChartDirective;

  // Line Chart
  lineChartLabels = ['Jan/2025', 'Feb/2025', 'Mar/2025', 'Apr/2025', 'May/2025', 'Jun/2025'];
  lineChartDatasets = [
    {
      data: [5, 12, 8, 10, 15, 7],
      label: 'Events',
      borderColor: '#1d4f91',
      backgroundColor: '#1d4f9150',
      fill: true
    }
  ]
  lineChartPlugins = [];
  lineChartLegend = true;
  lineChartOptions: ChartOptions<'line'> = {
    responsive: true
  };

  // Pie Chart
  pieChartOptions: ChartOptions<'pie'> = {
    responsive: true,
  };
  pieChartLabels = ['Society 1', 'Society 2', 'Society 3', 'Society 4'];
  pieChartDatasets = [{
    data: [22, 53, 17, 125],
    backgroundColor: ['#1d4f91', '#b8bb34', '#6cd3e6', '#3a79b8']
  }];
  pieChartLegend = true;
  pieChartPlugins = [];


  // Most Attended Events – Bar chart ranking events by attendance.
  public barChartLegend = true;
  public barChartPlugins = [];

  public barChartData: ChartConfiguration<'bar'>['data'] = {
    labels: ['ACM PST Summer 2024', 'IEEE Cup 2024', 'How to prepare for the Job market with Mohammad Abu-Hadhoud', 'Junior to Solver 5.0', 'CTF 2024'],
    datasets: [
      { data: [650, 600, 250, 102, 45], label: 'No of attendees' },
    ]
  };

  public barChartOptions: ChartConfiguration<'bar'>['options'] = {
    responsive: true,
    indexAxis: 'y',
    backgroundColor: '#1d4f9190',
    plugins: {
      legend: {
        display: true
      }
    },
    scales: {
      x: {
        stacked: true
      },
      y: {
        stacked: true
      }
    }
  };


  mostAttendedEventsOptions: ChartOptions<'bar'> = {
    responsive: true,
    indexAxis: 'y', // Makes it a horizontal bar chart
    plugins: {
      legend: {
        display: true
      }
    }
  };

  otherSocieties = [] as SocietyData[];
}
