import { api, setAuthToken, removeAuthToken, getAuthToken } from './api';
import type { ApiResponse } from './api';

// Auth types - Updated to match API ClientDTO structure
export interface LoginRequest {
  email: string;
  password: string;
}

export interface RegisterRequest {
  name: string;
  email: string;
  password: string;
  clientType?: number; // Based on ClientType enum: Owner=1, Admin=2, User=3
}

export interface ChangePasswordRequest {
  oldPassword: string;
  newPassword: string;
}

export interface User {
  idClient: number; // Updated to match API ClientDTO
  name: string;
  email: string;
  clientType: number;
  createdAt: string;
  updatedAt?: string;
}

export interface AuthResponse {
  token: string;
  user: User;
}

// Auth service - Real API only
export const authService = {
  // Login user
  login: async (credentials: LoginRequest): Promise<ApiResponse<AuthResponse>> => {
    const response = await api.post<AuthResponse>('/auth/login', credentials);
    
    if (response.success && response.data?.token) {
      setAuthToken(response.data.token);
    }
    
    return response;
  },

  // Register user - Using client endpoint for now
  register: async (userData: RegisterRequest): Promise<ApiResponse<AuthResponse>> => {
    // For now, we'll create a client and simulate auth response
    // This should be updated when proper auth endpoints are implemented in the API
    const clientData = {
      name: userData.name,
      email: userData.email,
      password: userData.password,
      clientType: userData.clientType || 3, // Default to User
    };

    const response = await api.post<User>('/client', clientData);
    
    if (response.success && response.data) {
      // Simulate auth response until proper auth is implemented
      const token = `temp-token-${response.data.idClient}-${Date.now()}`;
      setAuthToken(token);
      
      return {
        success: true,
        data: {
          token,
          user: response.data,
        },
      };
    }
    
    return {
      success: response.success,
      message: response.message,
      errors: response.errors,
    } as ApiResponse<AuthResponse>;
  },

  // Get current user - For now, we'll need to store user data locally
  me: async (): Promise<ApiResponse<User>> => {
    const token = getAuthToken();
    if (!token) {
      return {
        success: false,
        message: 'No authentication token found',
      };
    }

    // Since the API doesn't have a /me endpoint yet, we'll use stored user data
    // This should be updated when proper auth endpoints are implemented
    const storedUser = typeof window !== 'undefined' ? localStorage.getItem('currentUser') : null;
    
    if (storedUser) {
      try {
        const user = JSON.parse(storedUser);
        return {
          success: true,
          data: user,
        };
      } catch {
        return {
          success: false,
          message: 'Invalid stored user data',
        };
      }
    }
    
    return {
      success: false,
      message: 'User data not found',
    };
  },

  // Change password - Placeholder until API endpoint is implemented
  changePassword: async (): Promise<ApiResponse<void>> => {
    // This endpoint doesn't exist in the API yet
    return {
      success: false,
      message: 'Change password endpoint not implemented in API',
    };
  },

  // Logout user
  logout: (): void => {
    removeAuthToken();
    if (typeof window !== 'undefined') {
      localStorage.removeItem('currentUser');
    }
  },

  // Check if user is authenticated
  isAuthenticated: (): boolean => {
    return getAuthToken() !== null;
  },

  // Get current token
  getToken: (): string | null => {
    return getAuthToken();
  },
}; 