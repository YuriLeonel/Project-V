'use client';
import { TextField, Stack, Typography, Button, Box, Alert } from "@mui/material";
import { useState, useEffect, Suspense } from "react";
import { useRouter } from 'next/navigation';
import { useAuth } from '@/hooks/useAuth';

// Loading component
const LoadingSpinner = () => (
  <Box sx={{
    display: 'flex',
    flexDirection: 'column',
    alignItems: 'center',
    justifyContent: 'center',
    height: '100vh',
  }}>
    <div className="animate-spin rounded-full h-32 w-32 border-b-2 border-blue-600"></div>
  </Box>
);

// Inner login component
const LoginInner = () => {
    const [email, setEmail] = useState('');
    const [password, setPassword] = useState('');
    const [isLoading, setIsLoading] = useState(false);
    const router = useRouter();
    const { login, error, user, loading } = useAuth();

    // Get the intended client type from localStorage (set from home page)
    const [clientType, setClientType] = useState<'personal' | 'enterprise'>('personal');

    useEffect(() => {
        const selectedType = localStorage.getItem('selectedSessionType') as 'personal' | 'enterprise';
        if (selectedType) {
            setClientType(selectedType);
        }
    }, []);

    // Don't render the form if user is authenticated - let AuthProvider handle redirect
    if (user && !loading) {
        return <LoadingSpinner />;
    }

    const onLogin = async () => {
        if (!email || !password) {
            return;
        }

        setIsLoading(true);
        try {
            const success = await login(email, password);
            if (success) {
                // Remove the stored session type as it's no longer needed
                localStorage.removeItem('selectedSessionType');
                // AuthProvider will handle the redirect
            }
        } catch (error) {
            console.error('Login failed:', error);
        } finally {
            setIsLoading(false);
        }
    }

    return (
        <Box sx={{
            display: 'flex',
            flexDirection: 'column',
            alignItems: 'center',
            justifyContent: 'center',
            height: '100vh',
        }}>
            <Stack component="form" sx={{
                gap: 2,
                width: '100%',
                maxWidth: '400px',
                p: 3,
            }}>
                <Typography variant="h6">
                    Login - {clientType === 'personal' ? 'Pessoal' : 'Empresarial'}
                </Typography>
                
                {error && (
                    <Alert severity="error">{error}</Alert>
                )}
                
                <TextField 
                    label="E-mail" 
                    type="email" 
                    value={email} 
                    onChange={(e) => setEmail(e.target.value)}
                    disabled={isLoading}
                    required
                />
                <TextField 
                    label="Senha" 
                    type="password" 
                    value={password} 
                    onChange={(e) => setPassword(e.target.value)}
                    disabled={isLoading}
                    required
                />
                <Button 
                    variant="contained" 
                    color="primary" 
                    onClick={onLogin}
                    disabled={isLoading || !email || !password}
                >
                    {isLoading ? 'Entrando...' : 'Entrar'}
                </Button>
                <Button 
                    variant="text" 
                    color="primary" 
                    onClick={() => router.push('/signup')}
                >
                    Não tem uma conta? Cadastre-se
                </Button>
                <Button 
                    variant="text" 
                    color="secondary" 
                    onClick={() => router.push('/')}
                    size="small"
                >
                    Voltar ao início
                </Button>
            </Stack>
        </Box>
    );
}

// Main login component with Suspense wrapper
export default function Login() {
    return (
        <Suspense fallback={<LoadingSpinner />}>
            <LoginInner />
        </Suspense>
    );
} 