export interface EventSimpleRequest {
    id: string;  
    eventName: string;
    dateTime: Date;  
    locationString: string;
    approvalStatus: string;
    description: string;
    societyName: string;
}

// export interface EventDetailsRequest {
//     type?: string;
//     eventDate: string; 
//     startTime: string; 
//     endTime: string; 
//     societyDescription: string;
//     societyLogoId: string;
//     advisorId: string; 
//     advisorName: string;
//     advisorLogoId: string;
//     studentId: string; 
//     studentName: string;
//     studentEmail: string;
//     studentLogoId?: string;
//     studentDepartment: string;
//     joinYear: number;
//     studentRole: string;
//     joinedSocietiesNames: string[];
//     eventDTO: EventDTO;
//     eventRequestDTO: EventRequestDTO;
  
//     // Application history fields
//     isAdvisorApproved?: boolean;
//     isDeanAssistantApproved?: boolean;
//   }