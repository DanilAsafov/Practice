using System;
using System.Linq;
using System.Threading;

namespace task14;

public class DefiniteIntegral {
    public static double Solve(double a, double b, Func<double, double> function, double step, int threadsNumber) {
        if (threadsNumber <= 0)
            throw new ArgumentException("Ошибка: число потоков должно быть положительным", nameof(threadsNumber));
        if (step <= 0)
            throw new ArgumentException("Ошибка: шаг должен быть положительным", nameof(step));
            
        double total = 0.0;
        double length = b - a;
        
        if (length <= 0)
            return 0.0;

        double segmentLength = length / threadsNumber;

        using (Barrier barrier = new(threadsNumber + 1)) {
            Thread[] threads = new Thread[threadsNumber];
            
            foreach (int i in Enumerable.Range(0, threadsNumber)) {
                double startSegment = a + i * segmentLength;
                double endSegment = (i == threadsNumber - 1) ? b : startSegment + segmentLength;

                threads[i] = new Thread(() => {
                    double partial = LocalIntegral(startSegment, endSegment, function, step);
                    
                    double initialValue, newValue;
                    do {
                        initialValue = total;
                        newValue = initialValue + partial;
                    } while (initialValue != Interlocked.CompareExchange(ref total, newValue, initialValue));
                    
                    barrier.SignalAndWait();
                });
                
                threads[i].Start();
            }
            barrier.SignalAndWait();
        }
        return total;
    }

    private static double LocalIntegral(double start, double end, Func<double, double> function, double step) {
        double sum = 0.0;
        double current = start;
        double f_current = function(current);

        while (current < end) {
            double next = Math.Min(current + step, end);
            double f_next = function(next);
            sum += (f_current + f_next) * (next - current) / 2.0;
            current = next;
            f_current = f_next;
        }
        return sum;
    }
}
