export interface RecentlyJoinedMember {
    id: string;
    profileImageId: string;
    firstName: string;
    lastName: string;
    departmentName: string;
    joinDate: Date;
    societyName: string;
}

export interface RecentEvent {
    id: string;
    eventName: string;
    societyName: string;
    logoId: string;
    locationString: string;
    startTime: Date;
    isAdvised: boolean;
    isFinished: boolean;
}

export interface HomeStatistics {
    totalMembers: number;
    totalSocieties: number;
    totalCompletedEvents: number;
    totalAttendees: number;
}