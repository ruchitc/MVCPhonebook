export interface UpdatePassword {
    loginId: string | null | undefined,
    oldPassword: string,
    newPassword: string,
    confirmNewPassword: string,
}