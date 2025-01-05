import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../../environment/environment';
import { PaginatedProduct, Product } from '../models/product';

@Injectable({
  providedIn: 'root',
})
export class ProductsService {
  private baseUrl = `${environment.baseUrl}Products`;

  constructor(private http: HttpClient) {}

  getProducts(pageNumber: number, pageSize: number): Observable<PaginatedProduct> {
    return this.http.get<PaginatedProduct>(`${this.baseUrl}?pageNumber=${pageNumber}&pageSize=${pageSize}`);
  }

  getProductById(id: string): Observable<Product> {
    return this.http.get<Product>(`${this.baseUrl}/${id}`);
  }

  createProduct(category: Product): Observable<Product> {
    debugger;
    return this.http.post<Product>(this.baseUrl, category);
  }

  updateProduct(id: string, category: Product): Observable<void> {
    debugger;
    return this.http.put<void>(`${this.baseUrl}/${id}`, category);
  }

  deleteProduct(id: string): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${id}`);
  }
}
