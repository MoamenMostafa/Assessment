import { Routes } from '@angular/router';

export const routes: Routes = [
  {
    path: '',
    loadComponent: () =>
      import('./pages/components/categories/categories.component').then((m) => m.CategoriesComponent),
  },
  {
    path: 'products',
    loadComponent: () =>
      import('./pages/components/products/products.component').then((m) => m.ProductsComponent),
  },
  {
    path: 'add-category',
    loadComponent: () =>
      import('./pages/components/categories/add-category/add-category.component').then((m) => m.AddCategoryComponent),
  },
  {
    path: 'add-product',
    loadComponent: () =>
      import('./pages/components/products/add-product/add-product.component').then((m) => m.AddProductComponent), 
  },
];
