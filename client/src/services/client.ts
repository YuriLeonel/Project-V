import { api } from './api';
import type { ApiResponse } from './api';

// Client types - Updated to match API ClientDTO structure
export interface Client {
  idClient: number;
  name: string;
  email: string;
  password: string;
  clientType: number; // Owner=1, Admin=2, User=3
  createdAt: string;
  updatedAt?: string;
}

export interface CreateClientRequest {
  name: string;
  email: string;
  password: string;
  clientType: number;
}

export interface UpdateClientRequest {
  name?: string;
  email?: string;
  password?: string;
  clientType?: number;
}

export interface ClientQuery {
  page?: number;
  pageSize?: number;
  search?: string;
  sortBy?: string;
  sortOrder?: 'asc' | 'desc';
}

export interface PaginatedResponse<T> {
  data: T[];
  pagination: {
    page: number;
    pageSize: number;
    totalCount: number;
    totalPages: number;
  };
}

// Client service
export const clientService = {
  // Get all clients with pagination
  getAll: async (query?: ClientQuery): Promise<ApiResponse<PaginatedResponse<Client>>> => {
    const queryParams = new URLSearchParams();
    
    if (query?.page) queryParams.append('page', query.page.toString());
    if (query?.pageSize) queryParams.append('pageSize', query.pageSize.toString());
    if (query?.search) queryParams.append('search', query.search);
    if (query?.sortBy) queryParams.append('sortBy', query.sortBy);
    if (query?.sortOrder) queryParams.append('sortOrder', query.sortOrder);
    
    const endpoint = queryParams.toString() ? `/client?${queryParams.toString()}` : '/client';
    return await api.get<PaginatedResponse<Client>>(endpoint);
  },

  // Get client by ID
  getById: async (id: number): Promise<ApiResponse<Client>> => {
    return await api.get<Client>(`/client/${id}`);
  },

  // Create new client
  create: async (clientData: CreateClientRequest): Promise<ApiResponse<Client>> => {
    return await api.post<Client>('/client', clientData);
  },

  // Update client
  update: async (id: number, clientData: UpdateClientRequest): Promise<ApiResponse<Client>> => {
    return await api.patch<Client>(`/client/${id}`, clientData);
  },

  // Delete client
  delete: async (id: number): Promise<ApiResponse<void>> => {
    return await api.delete<void>(`/client/${id}`);
  },
}; 