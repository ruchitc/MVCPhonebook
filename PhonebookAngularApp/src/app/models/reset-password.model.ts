export interface ResetPassword {
    loginId: string | null | undefined,
    securityQuestionId_1: number,
    securityAnswer_1: string,
    securityQuestionId_2: number,
    securityAnswer_2: string,
    newPassword: string,
    confirmNewPassword: string,
}