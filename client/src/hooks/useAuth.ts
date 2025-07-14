'use client';

import { useState, useEffect, useCallback } from 'react';
import { authService } from '@/services/auth';
import type { User } from '@/services/auth';

interface UseAuthReturn {
  user: User | null;
  loading: boolean;
  error: string | null;
  login: (email: string, password: string) => Promise<boolean>;
  register: (name: string, email: string, password: string, clientType?: number) => Promise<boolean>;
  logout: () => void;
  refetchUser: () => Promise<void>;
  isAuthenticated: boolean;
}

export const useAuth = (): UseAuthReturn => {
  const [user, setUser] = useState<User | null>(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  const [mounted, setMounted] = useState(false);

  // Ensure we're mounted (client-side) before accessing localStorage
  useEffect(() => {
    setMounted(true);
  }, []);

  // Fetch current user - simplified to prevent race conditions
  const fetchUser = useCallback(async () => {
    if (!mounted) {
      return;
    }

    if (!authService.isAuthenticated()) {
      setUser(null);
      setLoading(false);
      return;
    }

    try {
      const response = await authService.me();
      
      if (response.success && response.data) {
        setUser(response.data);
        setError(null);
      } else {
        // Token might be invalid
        authService.logout();
        setUser(null);
        setError(response.message || 'Failed to fetch user');
      }
    } catch (err) {
      console.error('Error fetching user:', err);
      authService.logout();
      setUser(null);
      setError('Failed to fetch user');
    } finally {
      setLoading(false);
    }
  }, [mounted]);

  // Login function
  const login = async (email: string, password: string): Promise<boolean> => {
    setLoading(true);
    setError(null);

    try {
      const response = await authService.login({ email, password });
      
      if (response.success && response.data) {
        // Set user immediately to avoid race conditions
        setUser(response.data.user);
        setError(null);
        
        // Store user data for /me endpoint until proper auth is implemented
        if (typeof window !== 'undefined') {
          localStorage.setItem('currentUser', JSON.stringify(response.data.user));
        }
        
        return true;
      } else {
        setError(response.message || 'Login failed');
        setUser(null);
        return false;
      }
    } catch (err) {
      console.error('Login error:', err);
      setError('Login failed');
      setUser(null);
      return false;
    } finally {
      setLoading(false);
    }
  };

  // Register function
  const register = async (
    name: string, 
    email: string, 
    password: string, 
    clientType?: number
  ): Promise<boolean> => {
    setLoading(true);
    setError(null);

    try {
      const response = await authService.register({ 
        name, 
        email, 
        password,
        clientType: clientType || 3 // Default to User (3) - matches API enum
      });
      
      if (response.success && response.data) {
        // Set user immediately to avoid race conditions
        setUser(response.data.user);
        setError(null);
        
        // Store user data for /me endpoint until proper auth is implemented
        if (typeof window !== 'undefined') {
          localStorage.setItem('currentUser', JSON.stringify(response.data.user));
        }
        
        return true;
      } else {
        setError(response.message || 'Registration failed');
        setUser(null);
        return false;
      }
    } catch (err) {
      console.error('Registration error:', err);
      setError('Registration failed');
      setUser(null);
      return false;
    } finally {
      setLoading(false);
    }
  };

  // Logout function
  const logout = useCallback(() => {
    authService.logout();
    setUser(null);
    setError(null);
    // Clear any stored session type
    if (typeof window !== 'undefined') {
      localStorage.removeItem('selectedSessionType');
    }
  }, []);

  // Refetch user (useful after profile updates)
  const refetchUser = useCallback(async () => {
    setLoading(true);
    await fetchUser();
  }, [fetchUser]);

  // Initialize auth state only after mounting
  useEffect(() => {
    if (mounted) {
      fetchUser();
    }
  }, [mounted, fetchUser]);

  return {
    user,
    loading,
    error,
    login,
    register,
    logout,
    refetchUser,
    isAuthenticated: !!user,
  };
}; 