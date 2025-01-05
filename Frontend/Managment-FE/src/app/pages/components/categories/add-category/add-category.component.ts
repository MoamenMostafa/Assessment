import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { DropdownModule } from 'primeng/dropdown';
import { CommonModule } from '@angular/common';
import { Category, CategoryStatus } from '../../../models/category';
import { CategoriesService } from '../../../services/categories.service';
import { HttpClientModule } from '@angular/common/http'; 

@Component({
  selector: 'app-add-category',
  standalone: true,
  imports: [
    FormsModule,
    RouterModule,
    DropdownModule,
    CommonModule,
    HttpClientModule
  ],
  templateUrl: './add-category.component.html',
  styleUrls: ['./add-category.component.scss']
})
export class AddCategoryComponent {
  newCategory: Category = {
    id: '',
    name: '',
    description: '',
    parentCategoryId: null,
    status: CategoryStatus.Active, 
    createdDate: '',
    updatedDate: '',
    isEditable: true,
  };

 selectedStatus: CategoryStatus | null = null;

 categoryStatuses = [
   { label: 'Active', value: CategoryStatus.Active },
   { label: 'Inactive', value: CategoryStatus.Inactive }
 ];


  isLoading = false;
  errorMessage = ''; 

  constructor(
    private categoriesService: CategoriesService,
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
    if (!this.newCategory.name || this.newCategory.status === null) {
      console.error('Validation failed: Name and Status are required');
      return;
    }

    this.newCategory.id = this.generateGuid();
    const currentDate = new Date().toISOString();
    this.newCategory.createdDate = currentDate;
    this.newCategory.updatedDate = currentDate;

    this.categoriesService.createCategory(this.newCategory).subscribe({
      next: (createdCategory) => {
        console.log('Category created successfully:', createdCategory);
        this.router.navigate(['/']);
      },
      error: (error) => {
        console.error('Error creating category:', error);
      }
    });
  }
}
