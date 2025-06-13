'use client';
import { TextField, Stack, Typography, Button, Box } from "@mui/material";
import { useState } from "react";

export default function Login() {
    const [email, setEmail] = useState('');

    const onContinue = () => {
        // TODO: Validate if user exists
        // TODO: If user exists, redirect to home
        // TODO: If user does not exist, redirect to signup
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
        }}>
            <Typography variant="h6">Insira seu e-mail e entre ou cadastre-se</Typography>
            <TextField label="E-mail*" type="email" value={email} onChange={(e) => setEmail(e.target.value)} />
            <Button variant="contained" color="primary" onClick={onContinue}>Continuar</Button>
        </Stack>
        </Box>
    )
} 