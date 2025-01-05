import { Component, OnInit, inject } from '@angular/core';
import { Category, PaginatedCategory } from '../../models/category';
import { CategoriesService } from '../../services/categories.service';
import { HttpClientModule } from '@angular/common/http';
import { TableModule } from 'primeng/table';
import { CardModule } from 'primeng/card';
import { ToolbarModule } from 'primeng/toolbar';
import { ButtonModule } from 'primeng/button';
import { RouterModule } from '@angular/router';
import { DropdownModule } from 'primeng/dropdown';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-categories',
  imports: [
    HttpClientModule,
    TableModule,
    ToolbarModule,
    ButtonModule,
    RouterModule,
    CardModule,
    DropdownModule,
    FormsModule,
    ReactiveFormsModule,
    CommonModule,
  ],
  standalone: true,
  providers: [CategoriesService],
  templateUrl: './categories.component.html',
  styleUrls: ['./categories.component.scss'],
})
export class CategoriesComponent implements OnInit {
  private categoryService = inject(CategoriesService);

  categories: Category[] = [];
  totalCount: number = 0;
  pageNumber: number = 1;
  pageSize: number = 10;
  lastEditedCategoryId: string | null = null;

  categoryStatuses = [
    { label: 'Active', value: 0 },
    { label: 'Inactive', value: 1 },
  ];

  errorMessage: string | null = null;

  ngOnInit() {
    this.loadCategories();
  }

  get startRecord(): number {
    return (this.pageNumber - 1) * this.pageSize + 1;
  }

  get endRecord(): number {
    return Math.min(this.pageNumber * this.pageSize, this.totalCount);
  }

  loadCategories(): void {
    this.errorMessage = null;
    this.categoryService.getCategories(this.pageNumber, this.pageSize).subscribe(
      (response: PaginatedCategory) => {
        this.categories = response.data;
        this.totalCount = response.totalCount;
        this.pageNumber = response.pageNumber;
        this.pageSize = response.pageSize;
      },
      (error) => {
        console.error('Error loading categories:', error);
        this.errorMessage = 'Failed to load categories. Please try again later.';
      }
    );
  }

  editCategory(category: Category): void {
    category.isEditable = true;
    this.lastEditedCategoryId = category.id;
  }

  saveCategory(category: Category): void {
    category.isEditable = false;
    category.updatedDate = new Date().toISOString();

    this.categoryService.updateCategory(category.id, category).subscribe(
      () => {
        console.log('Category updated successfully');
        this.lastEditedCategoryId = category.id;
        this.loadCategories(); 
      },
      (error) => {
        console.error('Error updating category:', error);
      }
    );
  }

  cancelEdit(category: Category): void {
    category.isEditable = false;
    this.loadCategories(); 
  }

  deleteCategory(categoryId: string): void {
    this.categoryService.deleteCategory(categoryId).subscribe(
      () => {
        console.log('Category deleted successfully');
        this.loadCategories(); 
      },
      (error) => {
        console.error('Error deleting category:', error);
      }
    );
  }

  onPageChange(newPage: number): void {
    this.pageNumber = newPage;
    this.loadCategories();
  }
}
