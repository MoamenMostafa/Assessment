export interface Product {
    id: string; 
    name: string;
    description: string;
    price: number; 
    stockQuantity: number;
    categoryId: string;
    categoyName: string;
    imageUrl?: string | null; 
    createdDate: string; 
    updatedDate: string; 
    status: ProductStatus; 
    isEditable?: boolean; 

  }
  export interface PaginatedProduct {
    totalCount: number; 
    pageNumber: number; 
    pageSize: number; 
    data: Product[]; 
  }
  export enum ProductStatus {
    Active = 0,
    Inactive = 1,
    Discontinued = 2
  }
  