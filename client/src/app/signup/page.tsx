'use client';
import { TextField, Stack, Typography, Button, Box, Alert, MenuItem } from "@mui/material";
import { useState, useEffect } from "react";
import { useRouter } from 'next/navigation';
import { useAuth } from '@/hooks/useAuth';

export default function Signup() {
    const [email, setEmail] = useState('');
    const [name, setName] = useState('');
    const [password, setPassword] = useState('');
    const [clientType, setClientType] = useState(0); // 0 = Personal, 1 = Enterprise
    const [isLoading, setIsLoading] = useState(false);
    const router = useRouter();
    const { register, error, user } = useAuth();

    useEffect(() => {
        const selectedType = localStorage.getItem('selectedSessionType');
        if (selectedType) {
            setClientType(selectedType === 'personal' ? 0 : 1);
        }
    }, []);

    useEffect(() => {
        if (user) {
            return;
        }
    }, [user]);

    const onSignup = async () => {
        if (!email || !name || !password) {
            return;
        }

        setIsLoading(true);
        try {
            const success = await register(name, email, password, clientType);
            if (success) {
                localStorage.removeItem('selectedSessionType');
            }
        } catch (error) {
            console.error('Signup failed:', error);
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
                <Typography variant="h6">Cadastro</Typography>
                
                {error && (
                    <Alert severity="error">{error}</Alert>
                )}
                
                <TextField 
                    label="Nome" 
                    value={name} 
                    onChange={(e) => setName(e.target.value)}
                    disabled={isLoading}
                    required
                />
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
                <TextField
                    select
                    label="Tipo de Conta"
                    value={clientType}
                    onChange={(e) => setClientType(Number(e.target.value))}
                    disabled={isLoading}
                    required
                >
                    <MenuItem value={0}>Pessoal</MenuItem>
                    <MenuItem value={1}>Empresa</MenuItem>
                </TextField>
                <Button 
                    variant="contained" 
                    color="primary" 
                    onClick={onSignup}
                    disabled={isLoading || !email || !name || !password}
                >
                    {isLoading ? 'Cadastrando...' : 'Cadastrar'}
                </Button>
                <Button 
                    variant="text" 
                    color="primary" 
                    onClick={() => router.push('/login')}
                >
                    Já tem uma conta? Faça login
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