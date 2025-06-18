'use client';
import { TextField, Stack, Typography, Button, Box } from "@mui/material";
import { useState } from "react";
import { useRouter } from 'next/navigation';

export default function Signup() {
    const [email, setEmail] = useState('');
    const [name, setName] = useState('');
    const [password, setPassword] = useState('');
    const router = useRouter();

    const onSignup = async () => {
        try {
            // TODO: Replace with actual API call
            // For now, we'll simulate a successful signup
            const mockToken = 'mock-token-' + Date.now();
            localStorage.setItem('token', mockToken);
            router.push('/home');
        } catch (error) {
            console.error('Signup failed:', error);
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
                <Typography variant="h6">Cadastro</Typography>
                <TextField 
                    label="E-mail*" 
                    type="email" 
                    value={email} 
                    onChange={(e) => setEmail(e.target.value)}
                />
                <TextField 
                    label="Nome*" 
                    value={name} 
                    onChange={(e) => setName(e.target.value)}
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
                    onClick={onSignup}
                >
                    Cadastrar
                </Button>
                <Button 
                    variant="text" 
                    color="primary" 
                    onClick={() => router.push('/login')}
                >
                    Já tem uma conta? Faça login
                </Button>
            </Stack>
        </Box>
    );
} 