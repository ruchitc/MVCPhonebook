import { SecurityQuestion } from "./security-question.model";

export interface UserSecurityQuestions {
    loginId: string,
    securityQuestion_1: SecurityQuestion,
    securityQuestion_2: SecurityQuestion,
}