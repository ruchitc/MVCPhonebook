export interface UpdateContact {
    contactId: number;
    firstName: string;
    lastName: string;
    contactNumber: string;
    gender: string;
    countryId: number;
    stateId: number;
    isFavourite: boolean;
    email: string;
    company: string;
    image: any;
}