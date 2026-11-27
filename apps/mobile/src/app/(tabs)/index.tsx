import { HomeScreen } from '../../features/home/HomeScreen';
import { useCurrentEmployee } from '../../features/identity/currentEmployee';

export default function HomeRoute() {
  const employee = useCurrentEmployee();
  return employee ? <HomeScreen employee={employee} /> : null;
}
