'use client';

import { usePathname, useRouter, useSearchParams } from 'next/navigation';
import { useAuth } from '@/hooks/useAuth';
import { useEffect, useState, Suspense } from 'react';
import Navbar from '@/components/Navigation/Navbar';

// Pages that don't need authentication
const publicPages = ['/', '/login', '/signup'];

// Pages that should not show navigation
const noNavPages = ['/', '/login', '/signup'];

interface AuthProviderProps {
  children: React.ReactNode;
}

// Loading component
const LoadingSpinner = () => (
  <div className="min-h-screen flex items-center justify-center">
    <div className="animate-spin rounded-full h-32 w-32 border-b-2 border-blue-600"></div>
  </div>
);

// Inner component that uses useSearchParams
const AuthProviderInner: React.FC<AuthProviderProps> = ({ children }) => {
  const pathname = usePathname();
  const router = useRouter();
  const searchParams = useSearchParams();
  const { user, logout, loading } = useAuth();
  const [isRedirecting, setIsRedirecting] = useState(false);

  const isPublicPage = publicPages.includes(pathname);
  const showNavigation = !noNavPages.includes(pathname);

  // Handle authentication redirects with proper state management
  useEffect(() => {
    // Prevent multiple redirects
    if (isRedirecting) {
      return;
    }

    // Don't redirect while loading
    if (loading) {
      return;
    }

    // Case 1: User is not authenticated and trying to access protected page
    if (!user && !isPublicPage) {
      setIsRedirecting(true);
      const returnUrl = encodeURIComponent(pathname + (searchParams.toString() ? `?${searchParams.toString()}` : ''));
      const loginUrl = `/login?returnTo=${returnUrl}`;
      router.push(loginUrl);
      return;
    } 
    
    // Case 2: User is authenticated but on login/signup pages
    if (user && (pathname === '/login' || pathname === '/signup')) {
      setIsRedirecting(true);
      const returnTo = searchParams.get('returnTo');
      
      if (returnTo) {
        try {
          const decodedUrl = decodeURIComponent(returnTo);
          // Validate the return URL is safe (internal)
          if (decodedUrl.startsWith('/') && !decodedUrl.startsWith('//')) {
            router.push(decodedUrl);
            return;
          }
        } catch {
          // Invalid URL, fall through to default redirect
        }
      }
      
      router.push('/dashboard');
      return;
    }

    // Reset redirecting state when no redirect is needed
    setIsRedirecting(false);
  }, [loading, user, isPublicPage, pathname, router, searchParams, isRedirecting]);

  // Reset redirecting state when pathname changes
  useEffect(() => {
    setIsRedirecting(false);
  }, [pathname]);

  // Show loading while authenticating or redirecting
  if (loading || isRedirecting) {
    return <LoadingSpinner />;
  }

  // Don't render protected content if user is not authenticated
  if (!user && !isPublicPage) {
    return <LoadingSpinner />;
  }

  return (
    <>
      {showNavigation && <Navbar user={user} onLogout={logout} />}
      <main className={showNavigation ? '' : 'min-h-screen'}>
        {children}
      </main>
    </>
  );
};

// Main component with Suspense wrapper
const AuthProvider: React.FC<AuthProviderProps> = ({ children }) => {
  return (
    <Suspense fallback={<LoadingSpinner />}>
      <AuthProviderInner>{children}</AuthProviderInner>
    </Suspense>
  );
};

export default AuthProvider; 