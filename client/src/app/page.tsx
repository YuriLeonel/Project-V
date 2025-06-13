'use client';

import { Button, Stack, Typography } from '@mui/material';
import LoginIcon from '@mui/icons-material/Login';
import { useRouter } from 'next/navigation';

export default function Home() {
  const router = useRouter();
  
  const onLogin = () => {
    router.push('/login');
  }
  return (
    <Stack sx={{
      display: 'flex',
      flexDirection: 'column',
      alignItems: 'center',
      justifyContent: 'center',
      gap: 2,
      height: '100vh',
    }}>
      <Typography variant="h3">Hello, Lord!</Typography>
      <Button variant="contained" color="primary" endIcon={<LoginIcon />} sx={{
        width: '100%',
        maxWidth: '300px',
      }} 
      onClick={onLogin}>
        Login
      </Button>
    </Stack>
  );
}
