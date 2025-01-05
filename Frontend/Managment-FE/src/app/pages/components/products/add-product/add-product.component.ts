import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { DropdownModule } from 'primeng/dropdown';
import { CommonModule } from '@angular/common';
import { Product, ProductStatus } from '../../../models/product';
import { ProductsService } from '../../../services/products.service';

@Component({
  selector: 'app-add-product',
  standalone: true,
  imports: [
    FormsModule,
    RouterModule,
    DropdownModule,
    CommonModule,
  ],
  templateUrl: './add-product.component.html',
  styleUrls: ['./add-product.component.scss']
})
export class AddProductComponent {
  newProduct: Product = {
    id: '',
    name: '',
    description: '',
    price: 0,
    stockQuantity: 0,
    categoryId: '',
    categoyName: '',
    imageUrl: null,
    createdDate: '',
    updatedDate: '',
    status: ProductStatus.Active,
    isEditable: true,
  };

  productStatuses = [
    { label: 'Active', value: ProductStatus.Active },
    { label: 'Inactive', value: ProductStatus.Inactive },
    { label: 'Discontinued', value: ProductStatus.Discontinued },
  ];

  isLoading = false;
  errorMessage = '';

  constructor(
    private productsService: ProductsService,
    private router: Router
  ) {}

  generateGuid(): string {
    return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function (c) {
      const r = Math.random() * 16 | 0;
      const v = c === 'x' ? r : (r & 0x3 | 0x8);
      return v.toString(16);
    });
  }

  onSave() {
    debugger;
    if (!this.newProduct.name || this.newProduct.status === null) {
      this.errorMessage = 'Product Name and Status are required.';
      return;
    }

    this.errorMessage = '';
    this.isLoading = true;

    const currentDate = new Date().toISOString();
    this.newProduct.createdDate = currentDate;
    this.newProduct.updatedDate = currentDate;
    this.newProduct.id = this.generateGuid();

    this.productsService.createProduct(this.newProduct).subscribe({
      next: (createdProduct) => {
        console.log('Product created successfully:', createdProduct);
        this.isLoading = false;
        this.router.navigate(['/products']);
      },
      error: (error) => {
        console.error('Error creating product:', error);
        this.errorMessage = 'An error occurred while saving the product.';
        this.isLoading = false;
      }
    });
  }
}
