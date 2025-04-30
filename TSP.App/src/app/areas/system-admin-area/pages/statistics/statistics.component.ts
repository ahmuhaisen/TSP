import { Component, OnInit } from '@angular/core';
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

@Component({
  selector: 'app-statistics',
  imports: [
    NgClass,
    RouterLink,
    NzIconModule,
    NzButtonModule,
    NzDividerModule,
    NzEmptyModule,
    BaseChartDirective,
    ContainerBlockComponent
  ],
  templateUrl: './statistics.component.html',
  styleUrl: './statistics.component.css'
})
export class StatisticsComponent implements OnInit {
  // Loading states
  isLineChartEmpty = true;
  isPieChartEmpty = true;
  isBarChartEmpty = true;
  isTopSocietiesEmpty = true;

  // Line Chart
  lineChartLabels: string[] = [];
  lineChartDatasets: ChartConfiguration<'line'>['data']['datasets'] = [
    {
      data: [],
      label: 'Events',
      borderColor: '#1d4f91',
      backgroundColor: '#1d4f9150',
      fill: true
    }
  ];
  lineChartPlugins = [];
  lineChartLegend = true;
  lineChartOptions: ChartOptions<'line'> = {
    responsive: true
  };

  // Pie Chart
  pieChartOptions: ChartOptions<'pie'> = {
    responsive: true,
  };
  pieChartLabels: string[] = [];
  pieChartDatasets: ChartData<'pie'>['datasets'] = [{
    data: [],
    backgroundColor: ['#1d4f91', '#b8bb34', '#6cd3e6', '#3a79b8']
  }];
  pieChartLegend = true;
  pieChartPlugins = [];

  // Bar Chart
  public barChartLegend = true;
  public barChartPlugins = [];

  public barChartData: ChartConfiguration<'bar'>['data'] = {
    labels: [],
    datasets: [
      { data: [], label: 'No of attendees' },
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

  otherSocieties: SocietyData[] = [];

  constructor(private statisticsService: StatisticsService) {}

  ngOnInit() {
    this.loadEventsByMonth();
    this.loadTopSocietiesByMembers();
    this.loadTopEventsByAttendance();
    this.loadTopSocieties();
  }

  private loadEventsByMonth() {
    this.statisticsService.getEventsByMonth(6).subscribe(data => {
      this.lineChartLabels = data.map(item => item.month);
      this.lineChartDatasets[0].data = data.map(item => item.eventCount);
      this.isLineChartEmpty = data.length === 0 || data.every(item => item.eventCount === 0);
    });
  }

  private loadTopSocietiesByMembers() {
    this.statisticsService.getTopSocietiesByMembers(4).subscribe(data => {
      this.pieChartLabels = data.map(item => item.societyName);
      this.pieChartDatasets[0].data = data.map(item => item.membersCount);
      this.isPieChartEmpty = data.length === 0 || data.every(item => item.membersCount === 0);
    });
  }

  private loadTopEventsByAttendance() {
    this.statisticsService.getTopEventsByAttendance(5).subscribe(data => {
      this.barChartData.labels = data.map(item => item.eventName);
      this.barChartData.datasets[0].data = data.map(item => item.attendanceCount);
      this.isBarChartEmpty = data.length === 0 || data.every(item => item.attendanceCount === 0);
    });
  }

  private loadTopSocieties() {
    this.statisticsService.getTopSocieties(3).subscribe(data => {
      this.otherSocieties = data;
      this.isTopSocietiesEmpty = data.length === 0;
    });
  }
}
