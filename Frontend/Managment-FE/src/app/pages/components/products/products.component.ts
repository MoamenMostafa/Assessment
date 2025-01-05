import { Component, OnInit, inject } from '@angular/core';
import { HttpClientModule } from '@angular/common/http';
import { TableModule } from 'primeng/table';
import { CommonModule } from '@angular/common'; 
import { CardModule } from 'primeng/card';
import { ToolbarModule } from 'primeng/toolbar';
import { ButtonModule } from 'primeng/button';
import { DropdownModule } from 'primeng/dropdown';  
import { FormsModule, ReactiveFormsModule } from '@angular/forms'; 
import { CurrencyPipe, DatePipe } from '@angular/common';
import { PaginatedProduct, Product } from '../../models/product';
import { ProductsService } from '../../services/products.service';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'app-products',
  imports: [
    HttpClientModule,
    TableModule,
    ToolbarModule, 
    ButtonModule,  
    CommonModule,
    CardModule,
    RouterModule,
    DropdownModule,
    FormsModule, 
    ReactiveFormsModule, 
  ],
  standalone: true, 
  providers: [ProductsService, CurrencyPipe, DatePipe],  
  templateUrl: './products.component.html',
  styleUrls: ['./products.component.scss'],
})
export class ProductsComponent implements OnInit {
  private productService = inject(ProductsService);

  products: Product[] = [];
  totalCount: number = 0;
  pageNumber: number = 1;
  pageSize: number = 10;

  categoryStatuses = [
    { label: 'Active', value: 0 },
    { label: 'Inactive', value: 1 },
    { label: 'Discontinued', value: 2 }
  ];

  lastEditedProductId: string | null = null;

  ngOnInit() {
    this.loadProducts();
  }

  get startRecord(): number {
    return (this.pageNumber - 1) * this.pageSize + 1;
  }

  get endRecord(): number {
    return Math.min(this.pageNumber * this.pageSize, this.totalCount);
  }

  loadProducts(): void {
    this.productService.getProducts(this.pageNumber, this.pageSize).subscribe(
      (response: PaginatedProduct) => {
        this.products = response.data;
        this.totalCount = response.totalCount;
        this.pageNumber = response.pageNumber;
        this.pageSize = response.pageSize;
      },
      (error) => {
        console.error('Error loading products:', error);
      }
    );
  }

  editProduct(product: Product): void {
    debugger;
    product.isEditable = true;
    this.lastEditedProductId = product.id;  
  }

  saveProduct(product: Product): void {
    debugger;
    product.isEditable = false;
    product.updatedDate = new Date().toISOString();
  
    this.productService.updateProduct(product.id, product).subscribe(
      (response) => {
        console.log('Product updated:', response);
        this.loadProducts(); 
      },
      (error) => {
        console.error('Error updating product:', error);
      }
    );
    this.loadProducts(); 

  }
  
  cancelEditProduct(product: Product): void {
    product.isEditable = false;
    this.lastEditedProductId = null; 
    this.loadProducts(); 
  }

  
  deleteProduct(productId: string): void {
    this.productService.deleteProduct(productId).subscribe(
      () => {
        console.log('Product deleted successfully');
        this.loadProducts(); 
      },
      (error) => {
        console.error('Error deleting product:', error);
      }
    );
  }

  onPageChange(newPage: number): void {
    this.pageNumber = newPage;
    this.loadProducts();
  }
}
