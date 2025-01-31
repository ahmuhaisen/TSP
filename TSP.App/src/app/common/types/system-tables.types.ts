
export interface SchoolWithDepartmentsBasicDetails {
    id: number;
    name: string;
    departments: Department[]
}

export interface Department {
    id: number;
    name: string;
}