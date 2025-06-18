'use client';
import { TextField, Stack, Typography, Button, Box } from "@mui/material";
import { useState } from "react";
import { useRouter } from 'next/navigation';

export default function Login() {
    const [email, setEmail] = useState('');
    const [password, setPassword] = useState('');
    const router = useRouter();

    const onLogin = async () => {
        try {
            // TODO: Replace with actual API call
            // For now, we'll simulate a successful login
            const mockToken = 'mock-token-' + Date.now();
            localStorage.setItem('token', mockToken);
            router.push('/home');
        } catch (error) {
            console.error('Login failed:', error);
            // TODO: Add proper error handling
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
                <Typography variant="h6">Login</Typography>
                <TextField 
                    label="E-mail*" 
                    type="email" 
                    value={email} 
                    onChange={(e) => setEmail(e.target.value)}
                />
                <TextField 
                    label="Senha*" 
                    type="password" 
                    value={password} 
                    onChange={(e) => setPassword(e.target.value)}
                />
                <Button 
                    variant="contained" 
                    color="primary" 
                    onClick={onLogin}
                >
                    Entrar
                </Button>
                <Button 
                    variant="text" 
                    color="primary" 
                    onClick={() => router.push('/signup')}
                >
                    Não tem uma conta? Cadastre-se
                </Button>
            </Stack>
        </Box>
    );
} 