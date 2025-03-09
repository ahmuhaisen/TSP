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

export interface SocietyWithAdvisor extends Society {
    advisor: SocietyAdvisor;
}

export interface PostSociety {
    name: string;
    description: string;
    logoBase64: string;
    creationDate: string;
    themeColor: string;
    advisorId: string;
}

export interface SocietyAdvisor {
    id: string;
    fullName: string;
    logoId: string;
}

export interface SocietyMember {
    id: string;
    firstName: string;
    lastName: string;
    position: string;
    joinDate: Date;
    profileImageId: string;
}

export interface Member {
    id: string;
    name: string;
    position: string;
    memberSince: Date;
    imageUrl: string;
}