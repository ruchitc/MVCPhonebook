export interface PaginationApiResponse<T> {
    data: T;
    total: number;
    success: boolean;
    message: string;
}