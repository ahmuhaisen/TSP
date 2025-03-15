import { inject, Injectable } from '@angular/core';

import { DbService } from '../../../common/services/db.service';
import { HomeStatistics, RecentEvent, RecentlyJoinedMember } from '../api-interfaces/home.types';

@Injectable({
    providedIn: 'root'
})
export class HomeService {

    model = 'AdminArea/Home';
    // TODO: Get the current user id by Auth service
    tempUserId = '91A9273B-098E-4A90-7B57-08DD5EF9673F';

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
