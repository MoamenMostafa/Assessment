export interface Category {
  id: string;
  name: string;
  description?: string;
  parentCategoryId?: string | null;
  status: CategoryStatus | null; 
  createdDate: string;
  updatedDate: string;
  isEditable?: boolean; 
}

export interface PaginatedCategory {
  totalCount: number; 
  pageNumber: number; 
  pageSize: number; 
  data: Category[]; 
}

export enum CategoryStatus {
  Active = 0,
  Inactive = 1
}
