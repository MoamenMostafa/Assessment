import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Category, PaginatedCategory } from '../models/category';
import { environment } from '../../../environment/environment';

@Injectable({
  providedIn: 'root',
})
export class CategoriesService {
  private baseUrl = `${environment.baseUrl}categories`;

  constructor(private http: HttpClient) {}

  getCategories(pageNumber: number, pageSize: number): Observable<PaginatedCategory> {
    return this.http.get<PaginatedCategory>(`${this.baseUrl}?pageNumber=${pageNumber}&pageSize=${pageSize}`);
  }

  getCategoryById(id: string): Observable<Category> {
    return this.http.get<Category>(`${this.baseUrl}/${id}`);
  }

  createCategory(category: Category): Observable<Category> {
    return this.http.post<Category>(this.baseUrl, category);
  }

  updateCategory(id: string, category: Category): Observable<void> {
    return this.http.put<void>(`${this.baseUrl}/${id}`, category);
  }

  deleteCategory(id: string): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${id}`);
  }
}
