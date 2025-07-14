'use client';

import { TextField, Stack, Typography, Button, Box, Alert } from "@mui/material";
import { useState } from "react";
import { useRouter } from 'next/navigation';
import { authService } from '@/services/auth';

export default function ChangePassword() {
    const [oldPassword, setOldPassword] = useState('');
    const [newPassword, setNewPassword] = useState('');
    const [confirmPassword, setConfirmPassword] = useState('');
    const [isLoading, setIsLoading] = useState(false);
    const [error, setError] = useState<string | null>(null);
    const [success, setSuccess] = useState(false);
    const router = useRouter();

    const onChangePassword = async () => {
        if (!oldPassword || !newPassword || !confirmPassword) {
            setError('Todos os campos são obrigatórios');
            return;
        }

        if (newPassword !== confirmPassword) {
            setError('A nova senha e a confirmação devem ser iguais');
            return;
        }

        if (newPassword.length < 6) {
            setError('A nova senha deve ter pelo menos 6 caracteres');
            return;
        }

        setIsLoading(true);
        setError(null);

        try {
            // Note: Change password endpoint is not implemented in the API yet
            const response = await authService.changePassword();

            if (response.success) {
                setSuccess(true);
                // Clear form
                setOldPassword('');
                setNewPassword('');
                setConfirmPassword('');
                
                // Redirect after success
                setTimeout(() => {
                    router.push('/dashboard');
                }, 2000);
            } else {
                setError(response.message || 'Erro ao alterar senha');
            }
        } catch (error) {
            console.error('Change password failed:', error);
            setError('Erro ao alterar senha');
        } finally {
            setIsLoading(false);
        }
    };

    return (
        <Box sx={{
            display: 'flex',
            flexDirection: 'column',
            alignItems: 'center',
            justifyContent: 'center',
            minHeight: 'calc(100vh - 64px)', // Account for navbar height
            p: 2,
        }}>
            <Stack sx={{
                gap: 2,
                width: '100%',
                maxWidth: '400px',
                p: 3,
                backgroundColor: 'white',
                borderRadius: 2,
                boxShadow: 1,
            }}>
                <Typography variant="h5" component="h1" align="center">
                    Alterar Senha
                </Typography>
                
                {error && (
                    <Alert severity="error">{error}</Alert>
                )}
                
                {success && (
                    <Alert severity="success">
                        Senha alterada com sucesso! Redirecionando...
                    </Alert>
                )}
                
                <TextField 
                    label="Senha Atual*" 
                    type="password" 
                    value={oldPassword} 
                    onChange={(e) => setOldPassword(e.target.value)}
                    disabled={isLoading || success}
                    required
                />
                
                <TextField 
                    label="Nova Senha*" 
                    type="password" 
                    value={newPassword} 
                    onChange={(e) => setNewPassword(e.target.value)}
                    disabled={isLoading || success}
                    required
                    helperText="Mínimo 6 caracteres"
                />
                
                <TextField 
                    label="Confirmar Nova Senha*" 
                    type="password" 
                    value={confirmPassword} 
                    onChange={(e) => setConfirmPassword(e.target.value)}
                    disabled={isLoading || success}
                    required
                />
                
                <Stack direction="row" spacing={2}>
                    <Button 
                        variant="outlined" 
                        color="secondary" 
                        onClick={() => router.back()}
                        disabled={isLoading}
                        fullWidth
                    >
                        Cancelar
                    </Button>
                    
                    <Button 
                        variant="contained" 
                        color="primary" 
                        onClick={onChangePassword}
                        disabled={isLoading || success || !oldPassword || !newPassword || !confirmPassword}
                        fullWidth
                    >
                        {isLoading ? 'Alterando...' : 'Alterar Senha'}
                    </Button>
                </Stack>
            </Stack>
        </Box>
    );
} 