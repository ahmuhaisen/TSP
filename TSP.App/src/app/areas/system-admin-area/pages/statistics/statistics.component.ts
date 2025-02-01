import { Component } from '@angular/core';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzIconModule } from 'ng-zorro-antd/icon';
import { ContainerBlockComponent } from "../../../../components/container-block.component";
import { BaseChartDirective } from 'ng2-charts';
import { ChartConfiguration, ChartOptions } from 'chart.js';
import { NzDividerModule } from 'ng-zorro-antd/divider';
import { RouterLink } from '@angular/router';
import { NgClass } from '@angular/common';

@Component({
  selector: 'app-statistics',
  imports: [
    NgClass,
    RouterLink,
    NzIconModule,
    NzButtonModule,
    NzDividerModule,
    BaseChartDirective,
    ContainerBlockComponent
  ],
  templateUrl: './statistics.component.html',
  styleUrl: './statistics.component.css'
})
export class StatisticsComponent {
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
  pieChartLabels = ['ACM JU', 'IEEE CS JU', 'Waves JU', 'Other'];
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

  otherSocieties = [
    {
      id: '5ij-6kl-7mn-8op',
      name: 'IEEE CS JU',
      description: 'The IEEE Computer Society of Jordan',
      logoUrl: 'https://robohash.org/society1',
      NoOfMembers: 22,
      NoOfEvents: 53
    },
    {
      id: '1ab-2cd-3ef-4gh',
      name: 'ACM JU Student Chapter',
      description: 'A Chapter of the Association for Computing Machinery, interested in computer science and programming.',
      logoUrl: 'https://robohash.org/society2',
      NoOfMembers: 22,
      NoOfEvents: 53
    },
    {
      id: '9qr-0st-1uv-2wx',
      name: 'Linux Society JU',
      description: 'Linux Society of Jordan',
      logoUrl: 'https://robohash.org/society3',
      NoOfMembers: 22,
      NoOfEvents: 53
    }
  ];
}
