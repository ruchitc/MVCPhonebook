import { Country } from "./country.model";
import { State } from "./state.model";

export interface Contact {
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
    fileName: string;
    imageBytes: any;
    country: Country;
    state: State;
}