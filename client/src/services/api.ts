// API Configuration
const API_BASE_URL = process.env.NEXT_PUBLIC_API_URL || 'https://localhost:7111/api';

// Response types
interface ApiResponse<T = unknown> {
  data?: T;
  message?: string;
  errors?: string[];
  status?: number;
}

// Auth token management
const getAuthToken = (): string | null => {
  if (typeof window === 'undefined') return null;
  return localStorage.getItem('authToken');
};

const setAuthToken = (token: string): void => {
  if (typeof window !== 'undefined') {
    localStorage.setItem('authToken', token);
  }
};

const removeAuthToken = (): void => {
  if (typeof window !== 'undefined') {
    localStorage.removeItem('authToken');
  }
};

// Base fetch wrapper
const apiRequest = async <T = unknown>(
  endpoint: string,
  options: RequestInit = {}
): Promise<ApiResponse<T>> => {
  const url = `${API_BASE_URL}${endpoint}`;
  const token = getAuthToken();

  const config: RequestInit = {
    headers: {
      'Content-Type': 'application/json',
      ...(token && { Authorization: `Bearer ${token}` }),
      ...options.headers,
    },
    ...options,
  };

  try {
    const response = await fetch(url, config);
    
    // Handle non-JSON responses
    let data;
    const contentType = response.headers.get('content-type');
    if (contentType && contentType.includes('application/json')) {
      data = await response.json();
    } else {
      data = await response.text();
    }

    if (!response.ok) {
      return {
        status: response.status,
        message: data?.message || data || `HTTP error! status: ${response.status}`,
        errors: data?.errors || [],
      };
    }

    return {
      data,
      status: response.status,
    };
  } catch (error) {
    console.error('API Request failed:', error);
    return {
      message: error instanceof Error ? error.message : 'Network error occurred',
    };
  }
};

// HTTP Methods
const api = {
  get: <T = unknown>(endpoint: string): Promise<ApiResponse<T>> =>
    apiRequest<T>(endpoint, { method: 'GET' }),

  post: <T = unknown>(endpoint: string, data?: unknown): Promise<ApiResponse<T>> =>
    apiRequest<T>(endpoint, {
      method: 'POST',
      body: data ? JSON.stringify(data) : undefined,
    }),

  put: <T = unknown>(endpoint: string, data?: unknown): Promise<ApiResponse<T>> =>
    apiRequest<T>(endpoint, {
      method: 'PUT',
      body: data ? JSON.stringify(data) : undefined,
    }),

  patch: <T = unknown>(endpoint: string, data?: unknown): Promise<ApiResponse<T>> =>
    apiRequest<T>(endpoint, {
      method: 'PATCH',
      body: data ? JSON.stringify(data) : undefined,
    }),

  delete: <T = unknown>(endpoint: string): Promise<ApiResponse<T>> =>
    apiRequest<T>(endpoint, { method: 'DELETE' }),
};

export { api, setAuthToken, removeAuthToken, getAuthToken };
export type { ApiResponse }; 