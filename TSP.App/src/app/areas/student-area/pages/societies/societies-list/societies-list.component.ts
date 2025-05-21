import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { NzTabsModule } from 'ng-zorro-antd/tabs';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzIconModule } from 'ng-zorro-antd/icon';
import { NzModalModule } from 'ng-zorro-antd/modal';
import { NzTableModule } from 'ng-zorro-antd/table';
import { NzTagModule } from 'ng-zorro-antd/tag';
import { NzSelectModule } from 'ng-zorro-antd/select';
import { NzInputModule } from 'ng-zorro-antd/input';
import { NzDividerModule } from 'ng-zorro-antd/divider';
import { NzEmptyModule } from 'ng-zorro-antd/empty';
import { NzAvatarModule } from 'ng-zorro-antd/avatar';
import { SocietiesService } from '../../../services/societies.service';
import { MemberAssociatedSociety, SocietyJoinRequest, Society, JoinSocietyRequest } from '../../../api-interfaces/society.types';
import { NzMessageService } from 'ng-zorro-antd/message';
import { TruncatePipe } from "../../../../../common/pipes/truncate.pipe";
import { StudentsService } from '../../../services/students.service';
import { HttpClientModule } from '@angular/common/http';
import { environment } from '../../../../../../environments/environment';
@Component({
    selector: 'app-societies-list',
    standalone: true,
    imports: [
        CommonModule,
        RouterModule,
        ReactiveFormsModule,
        NzTabsModule,
        NzButtonModule,
        NzIconModule,
        NzModalModule,
        NzTableModule,
        NzTagModule,
        NzSelectModule,
        NzInputModule,
        NzDividerModule,
        NzEmptyModule,
        NzAvatarModule,
        TruncatePipe,
        HttpClientModule
    ],
    templateUrl: './societies-list.component.html',
    styleUrl: './societies-list.component.css'
})
export class SocietiesListComponent implements OnInit {
    belongingSocieties: MemberAssociatedSociety[] = [];
    otherSocieties: Society[] = [];
    joinRequests: SocietyJoinRequest[] = [];
    isLeaveSocietyPopupVisible = false;
    isJoinSocietyModalVisible = false;
    isLeaveSocietyLoading = false;
    isJoinSocietyLoading = false;
    societyToLeave: MemberAssociatedSociety | null = null;
    joinSocietyForm: FormGroup;
    baseSocietiesUrl: string = environment.gitHubSocietiesPicturesURL
    suggestedSections = ['Academic', 'Sports', 'Cultural', 'Technical', 'Social', 'Other'];

    constructor(
        private societiesService: SocietiesService,
        private studentsService: StudentsService,
        private fb: FormBuilder,
        private message: NzMessageService
    ) {
        this.joinSocietyForm = this.fb.group({
            societyId: ['', Validators.required],
            section: ['', Validators.required],
            motivation: ['', [Validators.required, Validators.minLength(50)]]
        });
    }

    ngOnInit(): void {
        this.loadSocieties();
        this.loadJoinRequests();
    }

    private loadSocieties(): void {
        this.studentsService.getBelongingSocieties().subscribe({
            next: (societies: MemberAssociatedSociety[]) => {
                this.belongingSocieties = societies;
                console.log(this.belongingSocieties)
            },
            error: (error: Error) => {
                this.message.error('Failed to load societies');
                console.error('Error loading societies:', error);
            }
        });

        this.studentsService.getCommitteeSocieties().subscribe({
            next: (societies: MemberAssociatedSociety[]) => {

                societies.forEach(e => e.isCommittee = true)
                this.belongingSocieties.push(...societies);
                console.log(this.belongingSocieties)
            },
            error: (error: Error) => {
                this.message.error('Failed to load societies');
                console.error('Error loading societies:', error);
            }
        });

        this.studentsService.getOtherSocieties().subscribe({
            next: (societies: Society[]) => {
                this.otherSocieties = societies;
            },
            error: (error: Error) => {
                this.message.error('Failed to load other societies');
                console.error('Error loading other societies:', error);
            }
        });
    }

    private loadJoinRequests(): void {
        this.studentsService.getJoinRequests().subscribe({
            next: (requests: SocietyJoinRequest[]) => {
                this.joinRequests = requests;
                console.log(this.joinRequests)
            },
            error: (error: Error) => {
                this.message.error('Failed to load join requests');
                console.error('Error loading join requests:', error);
            }
        });
    }

    showJoinSocietyModal(): void {
        // Check if there are available societies to join
        if (this.getAvailableSocieties().length === 0) {
            this.message.info('You have already requested to join all available societies');
            return;
        }
        this.joinSocietyForm.reset();
        this.isJoinSocietyModalVisible = true;
    }

    getAvailableSocieties(): Society[] {
        if (!this.joinRequests || !this.otherSocieties) {
            return this.otherSocieties || [];
        }

        const requestedSocietyNames = this.joinRequests.map(request => request.societyName);

        return this.otherSocieties.filter(society => !requestedSocietyNames.includes(society.name));
    }

    handleCancelJoinSociety(): void {
        this.isJoinSocietyModalVisible = false;
        this.joinSocietyForm.reset();
    }

    handleJoinSociety(): void {
        // Check if there are available societies to join
        if (this.getAvailableSocieties().length === 0) {
            this.message.info('No available societies to join');
            this.isJoinSocietyModalVisible = false;
            return;
        }

        if (this.joinSocietyForm.valid) {
            this.isJoinSocietyLoading = true;
            const formValue: JoinSocietyRequest = this.joinSocietyForm.value;
            this.societiesService.joinSociety(formValue).subscribe({
                next: () => {
                    this.message.success('Join request submitted successfully');
                    this.isJoinSocietyModalVisible = false;
                    this.joinSocietyForm.reset();
                    this.loadJoinRequests();
                },
                error: (error: Error) => {
                    this.message.error('Failed to submit join request');
                    console.error('Error submitting join request:', error);
                    this.isJoinSocietyLoading = false;
                },
                complete: () => {
                    this.isJoinSocietyLoading = false;
                }
            });
        }
    }

    leaveSociety(society: MemberAssociatedSociety): void {
        this.societyToLeave = society;
        this.isLeaveSocietyPopupVisible = true;

    }

    handleCancelLeaveSociety(): void {
        this.isLeaveSocietyPopupVisible = false;
        this.societiesService.leaveSociety(this.societyToLeave?.id || "").subscribe();

        this.societyToLeave = null;
    }

    handleOkLeaveSociety(): void {
        if (this.societyToLeave) {
            this.isLeaveSocietyLoading = true;
            this.societiesService.leaveSociety(this.societyToLeave.id).subscribe({
                next: () => {
                    this.message.success('Successfully left the society');
                    this.isLeaveSocietyPopupVisible = false;
                    this.societyToLeave = null;
                    this.loadSocieties();
                },
                error: (error: Error) => {
                    this.message.error('Failed to leave the society');
                    console.error('Error leaving society:', error);
                    this.isLeaveSocietyLoading = false;
                },
                complete: () => {
                    this.isLeaveSocietyLoading = false;
                }
            });
        }
    }

    selectSuggestedSection(section: string): void {
        this.joinSocietyForm.patchValue({ section });
    }

    getStatusColor(status: string): string {
        switch (status) {
            case 'Pending':
                return 'processing';
            case 'Accepted':
                return 'success';
            case 'Rejected':
                return 'error';
            default:
                return 'default';
        }
    }
}
