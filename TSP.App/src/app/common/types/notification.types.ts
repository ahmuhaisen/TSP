export interface INotification {
    username: string;
    message: string;
    date: Date;
    image: string | null;
    link: string | null;
}

export interface IGenericNotification {
    id: string;
    
    subject: string;
    body: string;
    createdAt: Date;

    imageId: string | null;
    isSeen: boolean;
}