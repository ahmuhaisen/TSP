export interface Society {
    id: string;
    name: string;
    description: string;
    logoId: string;
    creationDate: Date;
    themeColor: string;
    numberOfMembers: number;
}

export interface SocietyBasicDetails {
    id: string;
    name: string;
    description: string;
    logoId: string;
    creationDate: Date;
    themeColor: string;
}