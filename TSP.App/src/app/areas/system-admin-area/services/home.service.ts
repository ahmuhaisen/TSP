import { inject, Injectable } from '@angular/core';

import { DbService } from '../../../common/services/db.service';
import { HomeStatistics, RecentEvent, RecentlyJoinedMember } from '../api-interfaces/home.types';

@Injectable({
    providedIn: 'root'
})
export class HomeService {

    model = 'AdminArea/Home';
    // TODO: Get the current user id by Auth service
    tempUserId = '8ec53c2d-8abe-4c55-e810-08dd3fb8d60a';

    db = inject(DbService);

    recentEvents() {
        return this.db.getRequest<RecentEvent[]>(`${this.model}/recentEvents?advisorId=${this.tempUserId}`);
    }

    recentlyJoinedMembers() {
        return this.db.getRequest<RecentlyJoinedMember[]>(`${this.model}/recentlyJoinedMembers?advisorId=${this.tempUserId}`);
    }

    homeStatistics() {
        return this.db.getRequest<HomeStatistics>(`${this.model}/homeStatistics?advisorId=${this.tempUserId}`);
    }
}
