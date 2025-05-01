export interface EventBasicDetails {
    id: string;
    name: string;
    description: string;
    location: string;
    date: Date;
    startTime: Date;
    endTime: Date;
}

export interface PostAttendance {
    eventId: string;
    fullName: string;
    email: string;
    universityNumber: string;
    departmentId: string;
    phoneNumber?: string | null;
    notes?: string | null;
}